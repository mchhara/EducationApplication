using AutoMapper;
using AutoMapper.Execution;
using EducationAPI.Entities;
using EducationAPI.Exceptions;
using EducationAPI.Models.EducationalSubjectDto;
using EducationAPI.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace EducationAPI.Services.UserServices
{
    public class UserService: IUserService
    {
        private readonly EducationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly ILogger<UserService> _logger;

        public UserService(EducationDbContext dbContext, IMapper mapper, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, ILogger<UserService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _logger = logger;
        }

        public IEnumerable<UserResponseDto> GetAll()
        {
            _logger.LogWarning($"Get All Users action invoked");


            var users = _dbContext
                .Users
                .ToList();

            if (users == null) return null;

            var usersDto = _mapper.Map<List<UserResponseDto>>(users);

            return usersDto;
        }

        public UserResponseDto GetUser(int userId)
        {
            _logger.LogWarning($"Get User by id {userId} action invoked");

            var user = _dbContext
                .Users
                .FirstOrDefault(u => u.Id == userId);

            if (user == null) return null;

            var userDto = _mapper.Map<UserResponseDto>(user);
            
            return userDto;
        }

        public int Create(UserDto dto)
        {
            _logger.LogWarning($"Create User action invoked");

            var user = _mapper.Map<Entities.User>(dto);

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user.Id;
        }

        public bool Delete(int id)
        {
            _logger.LogWarning($"Delete User by id {id} action invoked");

            var user = _dbContext.Users.FirstOrDefault(i => i.Id == id);
            if (user is null) return false;

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
            return true;
        }

        public bool Update(UserDto dto, int id)
        {
            _logger.LogWarning($"Update User by id {id} action invoked");

            var user = _dbContext.Users.FirstOrDefault(i => i.Id == id);
            if (user is null) return false;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.DateOfBirth = dto.DateOfBirth;
            user.Email = dto.Email;
            user.Password = dto.Password;

            _dbContext.SaveChanges();
            return true;
        }

        public void RegisterUser(UserDto dto)
        {
            _logger.LogWarning($"Register new User action invoked");


            var newUser = new User()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                Email = dto.Email,
                RoleId = dto.RoleId
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            
            newUser.Password = hashedPassword;
             
            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

        }

        public string GenerateJwt(UserLogin dto)
        {
            _logger.LogWarning($"Login in User action invoked");

            var user = _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user is null)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

    }
}

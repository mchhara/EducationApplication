using AutoMapper;
using EducationAPI.Entities;
using EducationAPI.Models.EducationalSubjectDto;
using EducationAPI.Models.User;
using System.Linq;

namespace EducationAPI.Services.User
{
    public class UserService: IUserService
    {
        private readonly EducationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserService(EducationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<UserResponseDto> GetAll()
        {
            var users = _dbContext
                .Users
                .ToList();

            if (users == null) return null;

            var usersDto = _mapper.Map<List<UserResponseDto>>(users);

            return usersDto;
        }

        public UserResponseDto GetUser(int userId)
        {
            var user = _dbContext
                .Users
                .FirstOrDefault(u => u.Id == userId);

            if (user == null) return null;

            var userDto = _mapper.Map<UserResponseDto>(user);
            
            return userDto;
        }

        public UserResponseDto Create(UserDto dto)
        {
            var user = _mapper.Map<Entities.User>(dto);

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return _mapper.Map<UserResponseDto>(user);
        }

        public bool Delete(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(i => i.Id == id);
            if (user is null) return false;

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
            return true;
        }

        public bool Update(UserDto dto, int id)
        {
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
    }
}

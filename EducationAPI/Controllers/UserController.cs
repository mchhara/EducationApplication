using AutoMapper.Execution;
using EducationAPI.Entities;
using EducationAPI.Models.User;
using EducationAPI.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace EducationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Teacher")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserResponseDto>> GetAllUsers()
        {
            var users = _userService.GetAll();

            return users.IsNullOrEmpty() ? NotFound() : Ok(users);


        }


        [HttpGet("{userId:int}")]
        public IActionResult GetUser([FromRoute] int userId)
        {
            var user = _userService.GetUser(userId);

            if (user == null) return NotFound();

            return  Ok(user);
        }


        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = _userService.Create(dto);

            return Created($"{user}", null);
        }


        [HttpDelete("{userId:int}")]
        public IActionResult DeleteUser([FromRoute] int userId)
        {
            var isDeleted = _userService.Delete(userId);

            return isDeleted ? NoContent() : NotFound();
        }

        [HttpPut("{userId:int}")]
        public IActionResult UpdateUser([FromBody] UserDto request, [FromRoute] int userId)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = _userService.Update(request, userId);

            return user ? Ok() : NotFound();
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] UserDto dto)
        {
            _userService.RegisterUser(dto);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] UserLogin dto)
        {
            var token = _userService.GenerateJwt(dto);
            return Ok(token);
        }

    }
}

using AutoMapper.Execution;
using EducationAPI.Entities;
using EducationAPI.Models.User;
using EducationAPI.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EducationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

            return users.IsNullOrEmpty() ? NoContent() : Ok(users);


        }


        [HttpGet("{userId:int}")]
        public IActionResult GetUser([FromRoute] int userId)
        {
            var user = _userService.GetUser(userId);

            if (user == null) return NoContent();

            return  Ok(user);
        }


        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = _userService.Create(dto);

            return Ok(user);
        }


        [HttpDelete("{userId:int}")]
        public IActionResult DeleteUser([FromRoute] int userId)
        {
            var isDeleted = _userService.Delete(userId);
            return isDeleted ? NoContent() : NotFound();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateUser([FromBody] UserDto request, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = _userService.Update(request, id);
            return user ? Ok() : NotFound();
        }

    }
}

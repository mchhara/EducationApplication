using EducationAPI.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace EducationAPI.Services.User
{
    public interface IUserService
    {
        IEnumerable<UserResponseDto> GetAll();
        UserResponseDto GetUser(int userId);
        UserResponseDto Create(UserDto dto);
        bool Update(UserDto dto, int id);
        bool Delete(int id);
    }
}

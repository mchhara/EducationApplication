using EducationAPI.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace EducationAPI.Services.User
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetAll();
        UserDto GetUser(int userId);
        UserDto Create(UserDto dto);
        bool Update(UserDto dto, int id);
        bool Delete(int id);
    }
}

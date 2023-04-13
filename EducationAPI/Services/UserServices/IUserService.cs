using EducationAPI.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace EducationAPI.Services.UserServices
{
    public interface IUserService
    {
        IEnumerable<UserResponseDto> GetAll();
        UserResponseDto GetUser(int userId);
        int Create(UserDto dto);
        bool Update(UserDto dto, int id);
        bool Delete(int id);
        void RegisterUser(UserDto user);
        string GenerateJwt(UserLogin dto);
    }
}

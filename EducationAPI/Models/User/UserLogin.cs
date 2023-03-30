using EducationAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace EducationAPI.Models.User
{
    public class UserLogin
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

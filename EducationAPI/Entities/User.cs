﻿using System.ComponentModel.DataAnnotations;

namespace EducationAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public  Role Role { get; set; }
        public List<AssignmentUser> AssignmentsUser { get; set; }
        public List<EducationalSubjectUser> EducationalSubjectUsers { get; set; }


    }
}

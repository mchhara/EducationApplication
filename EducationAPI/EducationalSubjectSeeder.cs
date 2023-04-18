using EducationAPI.Entities;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace EducationAPI
{
    public class EducationalSubjectSeeder
    {
        private readonly EducationDbContext _dBContext;

        public EducationalSubjectSeeder(EducationDbContext dBContext)
        {
            _dBContext = dBContext;
        }

        public void Seed()
        {
            if (_dBContext.Database.CanConnect())
            {

                if (!_dBContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dBContext.Roles.AddRange(roles);
                    _dBContext.SaveChanges();
                }

                if (!_dBContext.EducationalSubjects.Any())
                {
                    var materials = GetEducationalMaterials();
                    _dBContext.EducationalSubjects.AddRange(materials);
                    _dBContext.SaveChanges();
                }
            }
        }


        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "Student"

                },
                new Role()
                {
                    Name = "Teacher"
                }
            };

            return roles;
        }

        private IEnumerable<EducationalSubject> GetEducationalMaterials()
        {
            var materials = new List<EducationalSubject>()
            {
                  new EducationalSubject()
                 {
                    Name = "MATH BOOK",
                    Description = "Materials for learning mathematics",
                    Assignments = new List<Assignment>
                    {

                        new Assignment()
                        {
                            Title = "adding two numbers",
                            Description ="add up the numbers: 129 + 120"
                        },

                        new Assignment()
                        {
                            Title = "subtraction two numbers ",
                            Description ="subtract two numbers from each other: 400 - 721"
                        },
                    },
                    
                },

                new EducationalSubject()
                {
                    Name = "HISTORY BOOK",
                    Description = "Materials for learning for students",
                    Assignments = new List<Assignment>
                    {

                        new Assignment()
                        {
                            Title = "US history",
                            Description ="Write the years of the Civil War."
                        },

                         new Assignment()
                        {
                            Title = "The Second World War",
                            Description ="Enter the date of the Battle of Britain"
                        },
                    },
                }
            };

            return materials;

        }
    }
}


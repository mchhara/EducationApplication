using EducationAPI.Entities;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace EducationAPI
{
    public class EducationalMaterialSeeder
    {
        private readonly EducationDbContext _dBContext;

        public EducationalMaterialSeeder(EducationDbContext dBContext)
        {
            _dBContext = dBContext;
        }

        public void Seed()
        {
            if (_dBContext.Database.CanConnect())
            {

                //var pendingMigrations = _dBContext.Database.GetPendingMigrations();

                //if (pendingMigrations != null && pendingMigrations.Any())
                //{
                //    _dBContext.Database.Migrate();
                //}


                if (!_dBContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dBContext.Roles.AddRange(roles);
                    _dBContext.SaveChanges();
                }

                if (!_dBContext.EducationalMaterials.Any())
                {
                    var materials = GetEducationalMaterials();
                    _dBContext.EducationalMaterials.AddRange(materials);
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
                },
                new Role()
                {
                    Name = "Admin"
                }
            };

            return roles;
        }

        private IEnumerable<EducationalMaterial> GetEducationalMaterials()
        {
            var materials = new List<EducationalMaterial>()
            {
                  new EducationalMaterial()
                 {
                    Name = "MATH BOOK",
                    Description = "Materials for learning mathematics",
                    SourceUrl = "",
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

                new EducationalMaterial()
                {
                    Name = "HISTORY BOOK",
                    Description = "Materials for learning for students",
                    SourceUrl = "",
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


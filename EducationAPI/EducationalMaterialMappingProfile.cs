using AutoMapper;
using EducationAPI.Entities;
using EducationAPI.Models;

namespace EducationAPI
{
    public class EducationalMaterialMappingProfile : Profile
    {
        public EducationalMaterialMappingProfile()
        {
            CreateMap<EducationalMaterial, EducationalMaterialDto>();
            CreateMap<Assignment, AssignmentDto>();


        }


    }
}

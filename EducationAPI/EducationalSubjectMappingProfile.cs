using AutoMapper;
using EducationAPI.Entities;
using EducationAPI.Models.Assignment;
using EducationAPI.Models.EducationalSubjectDto;
using EducationAPI.Models.User;

namespace EducationAPI
{
    public class EducationalSubjectMappingProfile : Profile
    {
        public EducationalSubjectMappingProfile()
        {
            CreateMap<EducationalSubject, EducationalSubjectDtoResponse>();
            CreateMap<EducationalSubjectDto, EducationalSubject>();

            CreateMap<Assignment, AssignmentResponseDto>();
            CreateMap<AssignmentDto, Assignment>();

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<AssignmentResult, AssignmentResultDto>();
            CreateMap<AssignmentResultDto, AssignmentResult>();

        }


    }
}

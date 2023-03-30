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

            CreateMap<Assignment, AssignmentDto>();
            CreateMap<AssignmentResponseDto, Assignment>();

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<AssignmentResult, AssignmentResultDto>();
            CreateMap<AssignmentResultDto, AssignmentResult>();

        }


    }
}

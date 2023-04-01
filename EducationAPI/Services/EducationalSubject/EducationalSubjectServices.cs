using System.Globalization;
using System.Linq;
using AutoMapper;
using EducationAPI.Entities;
using EducationAPI.Models.Assignment;
using EducationAPI.Models.EducationalSubjectDto;
using EducationAPI.Services.EducationalSubject;
using Microsoft.EntityFrameworkCore;

namespace EducationAPI.Services
{

    public class EducationalSubjectServices : IEducationalSubjectServices
    {
        private readonly EducationDbContext _dbContext;
        private readonly IMapper _mapper;

        public EducationalSubjectServices( EducationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public bool Delete(int id)
        {
            var educationalMaterial = _dbContext
                .EducationalSubjects
                .Include(a => a.Assignments)
                .FirstOrDefault(e => e.Id == id);

            if (educationalMaterial is null) return false;

            _dbContext.EducationalSubjects.Remove(educationalMaterial);
            _dbContext.SaveChanges();

            return true;
        }

        public EducationalSubjectDtoResponse GetById(int id)
        {
            var educationalMaterial = _dbContext
                .EducationalSubjects
                .FirstOrDefault(e => e.Id == id);

            if (educationalMaterial == null)
            {
                return null;
            }

            var result = _mapper.Map<EducationalSubjectDtoResponse>(educationalMaterial);
            return result;

        }


        public IEnumerable<EducationalSubjectDtoResponse> GetAll()
        {
            var educationalSubjects = _dbContext
                .EducationalSubjects
                .Include(a => a.Assignments)
                .ToList();

            var educationalSubjectDtos = _mapper.Map<List<EducationalSubjectDtoResponse>>(educationalSubjects);

            return educationalSubjectDtos;
        }


        public int Create(EducationalSubjectDto dto)
        {
            var educationalMaterial = _mapper.Map<Entities.EducationalSubject>(dto);
            _dbContext.EducationalSubjects.Add(educationalMaterial);
            _dbContext.SaveChanges();
            
            return educationalMaterial.Id;
        }


        public bool Update(EducationalSubjectDto dto, int id)
        {

            var educationalSubject = _dbContext
                .EducationalSubjects
                .FirstOrDefault(e => e.Id == id);

            if (educationalSubject == null) return false;
           
            educationalSubject.Name = dto.Name;
            educationalSubject.Description = dto.Description;
            
            _dbContext.SaveChanges();

            return true;
        }




        public AssignmentResponseDto AddAssigmentToSubject(AssignmentDto dto, int subjectId)
        {
            var subject = _dbContext
                .EducationalSubjects
                .FirstOrDefault(e => e.Id == subjectId);
            if(subject == null) { return null; }

            dto.EducationalSubjectId = subjectId;
            var assignment = _mapper.Map<Entities.Assignment>(dto);
            var taskResponse = _mapper.Map<AssignmentResponseDto>(assignment);
            
            _dbContext.Assignments.Add(assignment);
            _dbContext.SaveChanges();
            return taskResponse;

        }

        public bool DeleteAssignmentFromEducationalSubject(int subjectId, int taskId)
        {
            var task = _dbContext
                .Assignments
                .Where(a => a.AssignmentId == taskId && a.EducationalSubjectId == subjectId)
                .FirstOrDefault();

            if (task == null) return false;

            _dbContext.Assignments.Remove(task);
            _dbContext.SaveChanges();

            return true;
        }

        //check it
        public bool DeleteStudentFromEducationSubject(int subjectId, int studentId)
        {
            var subject = _dbContext
                .EducationalSubjects
                .FirstOrDefault(e => e.Id == subjectId);

            if (subject == null) return false;

            var student = _dbContext
                .Users
                .Where(s => s.EducationalSubjects.Contains(subject) && s.Id == studentId)
                .FirstOrDefault();

            if (student == null) return false;

            student.EducationalSubjects.Remove(subject);
            _dbContext.SaveChanges();

            return true;

        }
    }
}

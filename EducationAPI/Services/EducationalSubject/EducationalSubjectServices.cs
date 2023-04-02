using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using AutoMapper;
using AutoMapper.Execution;
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


        public bool UpdateEducationalSubject(EducationalSubjectDto dto, int id)
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

            var xd = _dbContext
                .EducationalSubjectUsers
                .FirstOrDefault(s => s.StudentId == studentId && s.EducationalSubjectId == subjectId);

            if (xd == null) return false;

            //var student = _dbContext
            //    .Users
            //    .Where(s => s.EducationalSubjectUser.Contains(subject) && s.Id == studentId)
            //    .FirstOrDefault();

            //if (student == null) return false;



            subject.EducationalSubjectUsers.Remove(xd);
            _dbContext.SaveChanges();

            return true;

        }

       public bool EditAssignment(int subjectId, int assignmentId, AssignmentDto dto)
        {
            var assignment = _dbContext
                .Assignments
                .Where(e =>e.AssignmentId == assignmentId && e.EducationalSubjectId == subjectId)
                .FirstOrDefault();

            if (assignment == null) return false;

            assignment.Title = dto.Title;
            assignment.Description = dto.Description;
            assignment.Deadline = dto.Deadline;
            assignment.EducationalSubjectId = dto.EducationalSubjectId;

            _dbContext.SaveChanges();

            return true;

        }


        public bool AddStudentToEducationalSubject(int subjectId, int studentId)
        {

            var subject = _dbContext
                .EducationalSubjects
                .Include(e => e.EducationalSubjectUsers)
                .FirstOrDefault(x => x.Id == subjectId);

            var student = _dbContext
                .Users
                .Include(e => e.EducationalSubjectUsers)
                .FirstOrDefault(x => x.Id == studentId);

            if (subject == null || student == null) { return false; }


            bool ifExistsInSubject = _dbContext.EducationalSubjectUsers
                .Any(s => s.StudentId == studentId && s.EducationalSubjectId == subjectId);

            if (ifExistsInSubject) { return false; }

            subject.EducationalSubjectUsers.Add(
                new EducationalSubjectUser()
                {
                    StudentId = studentId,
                    EducationalSubjectId = subjectId
                });

            _dbContext.SaveChanges();
            return true;
        }

        public bool AddStudentToAssignment(int assignmentId, int studentId)
        {
            var assignment = _dbContext
                .Assignments
                .FirstOrDefault(x => x.AssignmentId == assignmentId);

            var student = _dbContext
                .Users
                .Include(u => u.UserAssignments)
                .FirstOrDefault(x => x.Id == studentId);

            if (assignment == null || student == null) { return false; }


            bool ifExistsAssignment = _dbContext.Assignments
                .Any(s => s.StudentId == studentId && s.AssignmentId == assignmentId);

            if (ifExistsAssignment) { return false; }

            student.UserAssignments.Add(assignment);

            _dbContext.SaveChanges();
            return true;
        }

    }
}

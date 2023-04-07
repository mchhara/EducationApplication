using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using AutoMapper;
using AutoMapper.Execution;
using EducationAPI.Entities;
using EducationAPI.Models.Assignment;
using EducationAPI.Models.EducationalSubjectDto;
using EducationAPI.Models.User;
using EducationAPI.Services.EducationalSubject;
using Microsoft.EntityFrameworkCore;

namespace EducationAPI.Services
{

    public class EducationalSubjectServices : IEducationalSubjectServices
    {
        private readonly EducationDbContext _dbContext;
        private readonly IMapper _mapper;

        public EducationalSubjectServices(EducationDbContext dbContext, IMapper mapper)
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
            if (subject == null) { return null; }

            dto.EducationalSubjectId = subjectId;
            var assignment = _mapper.Map<Entities.Assignment>(dto);
            var taskResponse = _mapper.Map<AssignmentResponseDto>(assignment);

            _dbContext.Assignments.Add(assignment);
            _dbContext.SaveChanges();
            return taskResponse;

        }

        public bool DeleteAssignmentFromEducationalSubject(int taskId)
        {
            var task = _dbContext
                .Assignments
                .Where(a => a.Id == taskId)
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

            var user = _dbContext
                .EducationalSubjectUsers
                .FirstOrDefault(s => s.StudentId == studentId && s.EducationalSubjectId == subjectId);

            if (user == null) return false;

            //var student = _dbContext
            //    .Users
            //    .Where(s => s.EducationalSubjectUser.Contains(subject) && s.Id == studentId)
            //    .FirstOrDefault();

            //if (student == null) return false;



            subject.EducationalSubjectUsers.Remove(user);
            _dbContext.SaveChanges();

            return true;

        }

        public bool EditAssignment(int assignmentId, AssignmentDto dto)
        {
            var assignment = _dbContext
                .Assignments
                .Where(e => e.Id == assignmentId)
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
               .Include(u => u.AssignmentUsers)
               .FirstOrDefault(x => x.Id == assignmentId);

            var student = _dbContext
                .Users
                .Include(u => u.AssignmentsUser)
                .FirstOrDefault(x => x.Id == studentId);

            if (assignment == null || student == null) { return false; }


            bool ifExistsAssignment = _dbContext.AssignmentUsers
                .Any(s => s.StudentId == studentId && s.AssignmentId == assignmentId);

            if (ifExistsAssignment) { return false; }

            student.AssignmentsUser.Add(
                new AssignmentUser()
                {
                    StudentId = studentId,
                    AssignmentId = assignmentId
                });

            _dbContext.SaveChanges();
            return true;
        }


        public IEnumerable<UserGradeResult> GetUsersGrades()
        {
            var query = from user in _dbContext.Users
                        join assignmentUser in _dbContext.AssignmentUsers on user.Id equals assignmentUser.StudentId
                        join assignment in _dbContext.Assignments on assignmentUser.AssignmentId equals assignment.Id
                        join educationalSubject in _dbContext.EducationalSubjects on assignment.EducationalSubjectId equals educationalSubject.Id
                        join assignmentResult in _dbContext.AssignmentResults on assignmentUser.Id equals assignmentResult.AssignmentUserId
                        select new { user.Id, user.FirstName, educationalSubject.Name, assignmentResult.Grade, assignment.Title };

            if (query == null) return null;

            return query.Select(x => new UserGradeResult
            {
                UserID = x.Id,
                UserName = x.FirstName,
                Grade = x.Grade,
                EducationalMaterialName = x.Name,
                AssignmentName = x.Title
            }).ToList();
        }

        public IEnumerable<UserGradeResult> GetUserGrades(int userId)
        {
            var query = from user in _dbContext.Users
                        where user.Id == userId
                        join assignmentUser in _dbContext.AssignmentUsers on user.Id equals assignmentUser.StudentId
                        join assignment in _dbContext.Assignments on assignmentUser.AssignmentId equals assignment.Id
                        join educationalSubject in _dbContext.EducationalSubjects on assignment.EducationalSubjectId equals educationalSubject.Id
                        join assignmentResult in _dbContext.AssignmentResults on assignmentUser.Id equals assignmentResult.AssignmentUserId
                        select new { user.Id, user.FirstName, educationalSubject.Name, assignmentResult.Grade, assignment.Title };

            if (query == null) return null;

            var userGradeResults = query.Select(x => new UserGradeResult
            {
                UserID = x.Id,
                UserName = x.FirstName,
                Grade = x.Grade,
                EducationalMaterialName = x.Name,
                AssignmentName = x.Title
            });

            if (userGradeResults == null) { return null; }


            return userGradeResults;
        }

        public bool AddUserGradeToAssignment(int assignmentId, int userId, int gradeValue)
        {
            var assignmentUser = _dbContext.AssignmentUsers.FirstOrDefault(au => au.StudentId == userId && au.AssignmentId == assignmentId);
            if (assignmentUser == null)
            {
                return false;
            }

            var existingResult = _dbContext.AssignmentResults.FirstOrDefault(ar => ar.AssignmentUserId == assignmentUser.Id);
            
            if (existingResult == null)
            {
                return false;            }
            
            existingResult.Grade = gradeValue;

            _dbContext.SaveChanges();

            return true;
        }


        public bool DeleteUserGradeToAssignment(int assignmentId, int studentId)
        {
            var assignmentUser = _dbContext.AssignmentUsers.FirstOrDefault(au => au.StudentId == studentId && au.AssignmentId == assignmentId);

            if (assignmentUser == null)
            {
                return false;
            }

            var existingResult = _dbContext.AssignmentResults.FirstOrDefault(ar => ar.AssignmentUserId == assignmentUser.Id);
            
            if (existingResult == null)
            {
                return false;
            }

            existingResult.Grade = 0;

            _dbContext.SaveChanges();
            return true;
        }


        public IEnumerable<EducationalSubjectDtoResponse> GetAllUserSubjects(int studentId)
        {

            var educationalSubjects = _dbContext
             .EducationalSubjects
             .Include(e => e.Assignments)
             .ThenInclude(a => a.AssignmentUsers)
             .Where(e => e.EducationalSubjectUsers.Any(u => u.StudentId == studentId))
             .ToList();

            educationalSubjects.ForEach(e =>
            {
                e.Assignments = e.Assignments.Where(a => a.AssignmentUsers.Any(au => au.StudentId == studentId)).ToList();
            });

            var educationalSubjectDtos = _mapper.Map<List<EducationalSubjectDtoResponse>>(educationalSubjects);

            return educationalSubjectDtos;
        }
    }
}


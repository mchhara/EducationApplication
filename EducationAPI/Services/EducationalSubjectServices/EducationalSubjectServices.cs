using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Execution;
using EducationAPI.Entities;
using EducationAPI.Models.Assignment;
using EducationAPI.Models.AssignmentResult;
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
        private readonly ILogger<EducationalSubjectServices> _logger;

        public EducationalSubjectServices(EducationDbContext dbContext, IMapper mapper, ILogger<EducationalSubjectServices> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public bool Delete(int id)
        {
            _logger.LogWarning($"EducationalSubject with id: {id} DELETE action invoked");

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
            _logger.LogWarning($"EducationalSubject with id: {id} GET action invoked");

            var educationalMaterial = _dbContext
                .EducationalSubjects
                .Include(a => a.Assignments)
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
            _logger.LogWarning($"EducationalSubjects GETALL action invoked");

            var educationalSubjects = _dbContext
                .EducationalSubjects
                .Include(a => a.Assignments)
                .ToList();

            var educationalSubjectDtos = _mapper.Map<List<EducationalSubjectDtoResponse>>(educationalSubjects);

            return educationalSubjectDtos;
        }


        public int Create(EducationalSubjectDto dto)
        {
            _logger.LogWarning($"EducationalSubject POST action invoked");


            var educationalMaterial = _mapper.Map<Entities.EducationalSubject>(dto);
            _dbContext.EducationalSubjects.Add(educationalMaterial);
            _dbContext.SaveChanges();

            return educationalMaterial.Id;
        }


        public bool UpdateEducationalSubject(EducationalSubjectDto dto, int id)
        {
            _logger.LogWarning($"EducationalSubject with id: {id} PUT action invoked");

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
            _logger.LogWarning($"Add Assigment To EducationalSubject with EducationalSubjectId: {subjectId} invoked");

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
            _logger.LogWarning($"Delete Assignment From EducationalSubject with TaskId: {taskId} invoked");

            var task = _dbContext
                .Assignments
                .Where(a => a.Id == taskId)
                .FirstOrDefault();

            if (task == null) return false;

            _dbContext.Assignments.Remove(task);
            _dbContext.SaveChanges();

            return true;
        }

       
        public bool DeleteStudentFromEducationSubject(int subjectId, int studentId)
        {
            _logger.LogWarning($"Delete Student From EducationSubject with SubjectId: {subjectId} and StudentId {studentId} invoked");

            var subject = _dbContext
                .EducationalSubjects
                .Include(e => e.Assignments)
                .FirstOrDefault(e => e.Id == subjectId);

            if (subject == null) return false;

            var user = _dbContext
                .EducationalSubjectUsers
                .FirstOrDefault(s => s.StudentId == studentId && s.EducationalSubjectId == subjectId);

            if (user == null) return false;

            var subjectAssignments = _dbContext
                .Assignments
                .Include(au => au.AssignmentUsers)
                .Where(a => a.AssignmentUsers.Any(au => au.StudentId == studentId) && a.EducationalSubjectId == subjectId)
                .ToList();

            subject.EducationalSubjectUsers.Remove(user);

            foreach (var assignment in subjectAssignments)
            {
                var assignmentUser = assignment.AssignmentUsers.FirstOrDefault(au => au.StudentId == studentId);

                if (assignmentUser != null)
                {
                    assignment.AssignmentUsers.Remove(assignmentUser);
                }
            }

            _dbContext.SaveChanges();

            return true;

        }

        public bool EditAssignment(int assignmentId, AssignmentDto dto)
        {
            _logger.LogWarning($"Edit Assignment with AssignmentId: {assignmentId} invoked");

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
            _logger.LogWarning($"Add Student To EducationalSubject with SubjectId: {subjectId} and StudentId {studentId} invoked");

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
            _logger.LogWarning($"Add Student To Assignment with AssignmentId: {assignmentId} and StudentId {studentId} invoked");

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
            _logger.LogWarning($"Get All Users and Grades invoked");

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
            _logger.LogWarning($"Get All grades for specific user's id {userId} invoked");

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
            _logger.LogWarning($"Add User Grade To Assignment for UserId {userId}, AssignmentId {assignmentId} and GradeValue {gradeValue} invoked");


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
            _logger.LogWarning($"Delete User Grade From Assignment for UserId {studentId}, AssignmentId {assignmentId} invoked");

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
            _logger.LogWarning($"Get All UserSubjects for specific StudentId{studentId} action invoked");

            var educationalSubjects = _dbContext
             .EducationalSubjects
             .Where(e => e.EducationalSubjectUsers.Any(u => u.StudentId == studentId))
             .Include(e => e.Assignments)
             .ThenInclude(a => a.AssignmentUsers)
             .ToList();


            if (educationalSubjects.Count == 0)
            {
                return null;
            }

            educationalSubjects.ForEach(e =>
            {
                e.Assignments = e.Assignments.Where(a => a.AssignmentUsers.Any(au => au.StudentId == studentId)).ToList();
            });

            var educationalSubjectDtos = _mapper.Map<List<EducationalSubjectDtoResponse>>(educationalSubjects);

            return educationalSubjectDtos;
        }


        public IEnumerable<EducationalSubjectDtoResponse> GetUserSubject(int materialId, int studentId) 
        {
            _logger.LogWarning($"Get specific UserSubject id {materialId} for specific StudentId{studentId} action invoked");

            var educationalSubject = _dbContext
           .EducationalSubjects
           .Where(e => e.EducationalSubjectUsers.Any(u => u.StudentId == studentId) && e.Id == materialId)
           .Include(e => e.Assignments)
           .ThenInclude(a => a.AssignmentUsers)
           .ToList();

            if (educationalSubject.Count == 0)
            {
                return null;
            }

            educationalSubject.ForEach(e =>
            {
                e.Assignments = e.Assignments.Where(a => a.AssignmentUsers.Any(au => au.StudentId == studentId)).ToList();
            });

            var educationalSubjectDtos = _mapper.Map<List<EducationalSubjectDtoResponse>>(educationalSubject);

            return educationalSubjectDtos;
        }

        
        public int AddAssignmentSolution(AssignmentResultDto dto, int assignmentId, int studentId)
        {
            _logger.LogWarning($"Add AssignmentSolution by user id {studentId} for specific Assignment id {assignmentId} action invoked");


            var assignmentUser = _dbContext
               .AssignmentUsers
               .FirstOrDefault(e => e.StudentId == studentId && e.AssignmentId == assignmentId);
            if (assignmentUser == null) return 0;

            var ifResultExist = _dbContext
                .AssignmentResults
                .FirstOrDefault(ar => ar.AssignmentUserId == assignmentUser.Id);

            if (ifResultExist != null) return 0;

            var assignmentResult = _mapper.Map<Entities.AssignmentResult>(dto);
            
            assignmentResult.AssignmentUser = assignmentUser;

            _dbContext.AssignmentResults.Add(assignmentResult);
            _dbContext.SaveChanges();

            return assignmentResult.Id;  
        }


        public bool EditAssignmentSolution(AssignmentResultDto dto, int assignmentId, int studentId)
        {
            _logger.LogWarning($"Edit Assignment Solution by user id {studentId} for specific Assignment id {assignmentId} action invoked");

            var assignmentResult = _dbContext.AssignmentResults
                .Include(ar => ar.AssignmentUser)
                .ThenInclude(au => au.Assignment)
                .FirstOrDefault(ar => ar.AssignmentUser.StudentId == studentId && ar.AssignmentUser.AssignmentId == assignmentId);

            if (assignmentResult == null) return false;

            _mapper.Map(dto, assignmentResult);
            _dbContext.SaveChanges();

            return true;

        }


        public bool DeleteAssignmentSolution( int assignmentId, int studentId)
        {
            _logger.LogWarning($"Delete Assignment Solution by user id {studentId} for specific Assignment id {assignmentId} action invoked");

            var assignmentResult = _dbContext.AssignmentResults
                            .Include(ar => ar.AssignmentUser)
                            .ThenInclude(au => au.Assignment)
                            .FirstOrDefault(ar => ar.AssignmentUser.StudentId == studentId && ar.AssignmentUser.AssignmentId == assignmentId);

            if (assignmentResult == null) return false;

            _dbContext.AssignmentResults.Remove(assignmentResult);
            _dbContext.SaveChanges();

            return true;
        }

        public bool DeleteStudentFromAssignment(int assignmentId, int studentId)
        {
            _logger.LogWarning($"Delete Student by id {studentId} from specific Assignment id {assignmentId} action invoked");

            var assignment = _dbContext
                           .Assignments
                           .FirstOrDefault(e => e.Id == assignmentId);

            if (assignment == null) return false;

            var user = _dbContext
                .AssignmentUsers
                .FirstOrDefault(s => s.StudentId == studentId && s.AssignmentId == assignmentId);

            if (user == null) return false;


            assignment.AssignmentUsers.Remove(user);
            _dbContext.SaveChanges();

            return true;
        }

    }
}


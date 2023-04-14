using AutoMapper;
using EducationAPI.Entities;
using EducationAPI.Models.Assignment;
using EducationAPI.Models.AssignmentResult;
using EducationAPI.Models.EducationalSubjectDto;
using EducationAPI.Services;
using EducationAPI.Services.EducationalSubject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Teacher")]
    public class EducationalSubjectController : ControllerBase
    {
        private readonly IEducationalSubjectServices _educationalSubjectServices;


        public EducationalSubjectController(IEducationalSubjectServices educationalSubjectServices)
        {
            _educationalSubjectServices = educationalSubjectServices;
        }



        // Endpoints for teacher's panel

        [HttpGet]
        public ActionResult<IEnumerable<EducationalSubjectDtoResponse>> GetAllSubjects()
        {
            var educationalMaterialDtos = _educationalSubjectServices.GetAll();

            if (educationalMaterialDtos == null)
            {
                return NotFound();
            }

            return Ok(educationalMaterialDtos);
        }


        [HttpGet("{id:int}")]
        public ActionResult GetSubject([FromRoute] int id)
        {
            var educationalMaterial = _educationalSubjectServices.GetById(id);

            if (educationalMaterial == null)
            {
                return NotFound();
            }

            return Ok(educationalMaterial);
        }

        [HttpPost]
        public ActionResult CreateEducationalSubject([FromBody] EducationalSubjectDto dto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = _educationalSubjectServices.Create(dto);


            return Created($"{id}", null);
        }


        [HttpPost("{subjectId:int}/Assignment")]
        public ActionResult AddAssigmentToSubject([FromBody] AssignmentDto dto, [FromRoute] int subjectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = _educationalSubjectServices.AddAssigmentToSubject(dto, subjectId);

            if (id == null) return NotFound();

            return Created($"{id}", null);
        }



        [HttpDelete("{id:int}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var isDeleted = _educationalSubjectServices.Delete(id);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound();
        }


        [HttpPut("{id:int}")]
        public ActionResult UpdateEducationalSubject([FromBody] EducationalSubjectDto dto, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = _educationalSubjectServices.UpdateEducationalSubject(dto, id);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }


        [HttpDelete("/Assignment/{assignmentId:int}")]
        public ActionResult DeleteAssignmentFromEducationSubject([FromRoute] int assignmentId)
        {
            var isDeleted = _educationalSubjectServices.DeleteAssignmentFromEducationalSubject(assignmentId);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpDelete("{subjectId:int}/Student/{studentId}")]
        public ActionResult DeleteStudentFromEducationSubject([FromRoute] int subjectId, [FromRoute] int studentId)
        {
            var isDeleted = _educationalSubjectServices.DeleteStudentFromEducationSubject(subjectId, studentId);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpDelete("Assignment/{assignmentId:int}/Student/{studentId}")]
        public ActionResult DeleteStudentFromAssignment([FromRoute] int assignmentId, [FromRoute] int studentId)
        {
            var isDeleted = _educationalSubjectServices.DeleteStudentFromAssignment(assignmentId, studentId);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound();
        }


        [HttpPut("/Assignment/{assignmentId}")]
        public ActionResult EditAssignment([FromRoute] int assignmentId, [FromBody] AssignmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = _educationalSubjectServices.EditAssignment(assignmentId, dto);

            if (!result)
            {
                return NotFound();
            }

            return Ok();

        }

        [HttpPut("{subjectId}/Student/{studentId}")]
        public ActionResult AddStudentToEducationalSubject([FromRoute] int subjectId, [FromRoute] int studentId)
        {
           
            var result = _educationalSubjectServices.AddStudentToEducationalSubject(subjectId, studentId);

            if (!result)
            {
                return NotFound();
            }

            return Ok();

        }

        [HttpPut("/Task{assignmentId}/Student{studentId}")]
        public ActionResult AddStudentToAssignment([FromRoute] int assignmentId, [FromRoute] int studentId)
        {

            var result = _educationalSubjectServices.AddStudentToAssignment(assignmentId, studentId);

            if (!result)
            {
                return NotFound();
            }

            return Ok();

        }
        //GradesEndpoints

        [HttpGet("/UserGrades")]
        public ActionResult<IEnumerable<EducationalSubjectDtoResponse>> GetAllUsersAndGrades()
        {
            var result = _educationalSubjectServices.GetUsersGrades();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("/UserGrades/{userId}")]
        public ActionResult GetUserAndGrades(int userId)
        {
            var result = _educationalSubjectServices.GetUserGrades(userId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpPut("/Assignment/{assignmentId}/Student/{studentId}/Grade/{gradeValue}")]
        public ActionResult AddUserGradeToAssignment(int assignmentId, int studentId, int gradeValue)
        {
            var result = _educationalSubjectServices.AddUserGradeToAssignment(assignmentId, studentId, gradeValue);

            if (result == false)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("/Assignment/{assignmentId}/Student/{studentId}")]
        public ActionResult DeleteUserGradeToAssignment(int assignmentId, int studentId)
        {
            var isDeleted = _educationalSubjectServices.DeleteUserGradeToAssignment(assignmentId, studentId);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound();
        }

        // Endpoints for student's panel

        [HttpGet("/StudentMaterials/Student/{studentId}")]
        [Authorize(Roles = "Student")]

        public ActionResult<IEnumerable<EducationalSubjectDtoResponse>> GetAllUserSubjects([FromRoute] int studentId)
        {
            var educationalMaterialDtos = _educationalSubjectServices.GetAllUserSubjects(studentId);

            if (educationalMaterialDtos == null)
            {
                return NotFound();
            }

            return Ok(educationalMaterialDtos);
        }

        [HttpGet("/StudentMaterials/{materialId}/Student/{studentId}")]
        [Authorize(Roles = "Student")]
        public ActionResult<IEnumerable<EducationalSubjectDtoResponse>> GetUserSubject([FromRoute] int materialId, [FromRoute] int studentId)
        {
            var educationalMaterialDtos = _educationalSubjectServices.GetUserSubject(materialId, studentId);

            if (educationalMaterialDtos == null)
            {
                return NotFound();
            }

            return Ok(educationalMaterialDtos);
        }

        [HttpPost("/Assignment/{assignmentId}/Student/{studentId}/AssignmentSolution")]
        [Authorize(Roles = "Student")]
        public ActionResult AddAssignmentSolution([FromBody] AssignmentResultDto dto,[FromRoute] int  assignmentId, [FromRoute] int studentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = _educationalSubjectServices.AddAssignmentSolution(dto, assignmentId, studentId);

            if(id == 0) return NotFound();

            return Created($"{id}", null);
        }

        [HttpPut("/Assignment/{assignmentId}/Student/{studentId}/AssignmentSolution")]
        [Authorize(Roles = "Student")]
        public ActionResult EditAssignmentSolution([FromBody] AssignmentResultDto dto, [FromRoute] int assignmentId, [FromRoute] int studentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _educationalSubjectServices.EditAssignmentSolution(dto, assignmentId, studentId);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }


        [HttpDelete("/Assignment/{assignmentId}/Student/{studentId}/AssignmentSolution")]
        [Authorize(Roles = "Student")]
        public ActionResult DeleteAssignmentSolution([FromRoute] int assignmentId, [FromRoute] int studentId)
        {

            var result = _educationalSubjectServices.DeleteAssignmentSolution(assignmentId, studentId);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}

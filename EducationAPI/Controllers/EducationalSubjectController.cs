using AutoMapper;
using EducationAPI.Entities;
using EducationAPI.Models.Assignment;
using EducationAPI.Models.EducationalSubjectDto;
using EducationAPI.Services;
using EducationAPI.Services.EducationalSubject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
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


        [HttpPut("/Assignment/{assignmentId}/User/{userId}/Grade/{gradeValue}")]
        public ActionResult AddUserGradeToAssignment(int assignmentId, int userId, int gradeValue)
        {
            var result = _educationalSubjectServices.AddUserGradeToAssignment(assignmentId, userId, gradeValue);

            if (result == false)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}

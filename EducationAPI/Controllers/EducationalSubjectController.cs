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


        [HttpPost("{subjectId:int}")]
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
        public ActionResult Update([FromBody] EducationalSubjectDto dto, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = _educationalSubjectServices.Update(dto, id);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }


        [HttpDelete("{subjectId:int}/assignment/{assignmentId:int}")]
        public ActionResult DeleteAssignmentFromEducationSubject([FromRoute] int subjectId, [FromRoute] int assignmentId)
        {
            var isDeleted = _educationalSubjectServices.DeleteAssignmentFromEducationalSubject(subjectId, assignmentId);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpDelete("{subjectId:int}/student{studentId}")]
        public ActionResult DeleteStudentFromEducationSubject([FromRoute] int subjectId, [FromRoute] int studentId)
        {
            var isDeleted = _educationalSubjectServices.DeleteStudentFromEducationSubject(subjectId, studentId);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound();
        }








    }
}

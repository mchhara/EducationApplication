using AutoMapper;
using EducationAPI.Entities;
using EducationAPI.Models;
using EducationAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationAPI.Controllers
{
    [Route("api/material")]
    [ApiController]
    public class EducationalMaterialController: ControllerBase
    {
        private readonly IEducationalMaterialServices _educationalMaterialServices;


        public EducationalMaterialController(IEducationalMaterialServices educationalMaterialServices )
        {
            _educationalMaterialServices = educationalMaterialServices;
        }


        [HttpGet]
        public ActionResult<IEnumerable<EducationalMaterialDto>> GetAll()
        {
           var educationalMaterialDtos = _educationalMaterialServices.GetAll();
           return Ok(educationalMaterialDtos);
        }


        [HttpGet("{id}")]
        public ActionResult Get([FromRoute] int id)
        {
            var educationalMaterial = _educationalMaterialServices.GetById(id);

            if (educationalMaterial == null)
            {
                return NotFound();
            }

            return Ok(educationalMaterial);
        }

        [HttpPost]
        public ActionResult CreateEducationalMaterial([FromBody] CreateEducationalMaterialDto dto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = _educationalMaterialServices.Create(dto);
           

            return Created($"/api/material/{id}",null);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var isDeleted = _educationalMaterialServices.Delete(id);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound();
        }


        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateEducationalMaterialDto dto, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = _educationalMaterialServices.Update(dto, id);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}

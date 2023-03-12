using AutoMapper;
using EducationAPI.Entities;
using EducationAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationAPI.Controllers
{
    [Route("api/material")]
    [ApiController]
    public class EducationalMaterialController: ControllerBase
    {
        private readonly EducationDbContext _dbContext;
        private readonly IMapper _mapper;


        public EducationalMaterialController(EducationDbContext dbContext, IMapper mapper )
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<EducationalMaterialDto>> GetAll()
        {
            var educationalMaterial = _dbContext
                .EducationalMaterials
                .Include(a => a.Assignments)
                .ToList();

            var educationalMaterialDtos = _mapper.Map<List<EducationalMaterialDto>>(educationalMaterial);

            return Ok(educationalMaterialDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<EducationalMaterialDto>> Get([FromRoute] int id)
        {
            var educationalMaterial = _dbContext
                .EducationalMaterials
                .Include(a => a.Assignments)
                .FirstOrDefault(e => e.Id == id);

            var educationalMaterialDto = _mapper.Map<EducationalMaterialDto>(educationalMaterial);


            if (educationalMaterial == null)
            {
                return NotFound();
            }
            return Ok(educationalMaterialDto);
        }

    }
}

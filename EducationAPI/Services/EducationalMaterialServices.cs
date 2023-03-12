using System.Globalization;
using AutoMapper;
using EducationAPI.Entities;
using EducationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationAPI.Services
{
    public interface IEducationalMaterialServices
    {
        EducationalMaterialDto GetById(int id);
        IEnumerable<EducationalMaterialDto> GetAll();
        int Create(CreateEducationalMaterialDto dto);
        bool Delete(int id);
        bool Update(UpdateEducationalMaterialDto dto, int id);
    }

    public class EducationalMaterialServices : IEducationalMaterialServices
    {
        private readonly EducationDbContext _dbContext;
        private readonly IMapper _mapper;

        public EducationalMaterialServices( EducationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public bool Delete(int id)
        {
            var educationalMaterial = _dbContext
                .EducationalMaterials
                .Include(a => a.Assignments)
                .FirstOrDefault(e => e.Id == id);

            if (educationalMaterial is null) return false;

            _dbContext.EducationalMaterials.Remove(educationalMaterial);
            _dbContext.SaveChanges();

            return true;
        }

        public EducationalMaterialDto GetById(int id)
        {
            var educationalMaterial = _dbContext
                .EducationalMaterials
                .FirstOrDefault(e => e.Id == id);

            if (educationalMaterial == null)
            {
                return null;
            }

            var result = _mapper.Map<EducationalMaterialDto>(educationalMaterial);
            return result;

        }


        public IEnumerable<EducationalMaterialDto> GetAll()
        {
            var educationalMaterial = _dbContext
                .EducationalMaterials
                .Include(a => a.Assignments)
                .ToList();

            var educationalMaterialDtos = _mapper.Map<List<EducationalMaterialDto>>(educationalMaterial);

            return educationalMaterialDtos;
        }


        public int Create(CreateEducationalMaterialDto dto)
        {
            var educationalMaterial = _mapper.Map<EducationalMaterial>(dto);
            _dbContext.EducationalMaterials.Add(educationalMaterial);
            _dbContext.SaveChanges();
            
            return educationalMaterial.Id;
        }


        public bool Update(UpdateEducationalMaterialDto dto, int id)
        {

            var educationalMaterial = _dbContext
                .EducationalMaterials
                .FirstOrDefault(e => e.Id == id);

            if (educationalMaterial == null) return false;
           
            educationalMaterial.Name = dto.Name;
            educationalMaterial.Description = dto.Description;
            
            _dbContext.SaveChanges();

            return true;
        }
    }
}

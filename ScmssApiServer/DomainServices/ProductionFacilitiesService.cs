using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.DomainServices
{
    public class ProductionFacilitiesService : IProductionFacilitiesService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductionFacilitiesService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ProductionFacilityDto> AddAsync(ProductionFacilityInputDto dto)
        {
            var facility = _mapper.Map<ProductionFacility>(dto);
            _dbContext.Add(facility);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<ProductionFacilityDto>(facility);
        }

        public async Task<ProductionFacilityDto?> GetAsync(int id)
        {
            ProductionFacility? facility = await _dbContext.ProductionFacilities
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
            return _mapper.Map<ProductionFacilityDto?>(facility);
        }

        public async Task<IList<ProductionFacilityDto>> GetManyAsync(SimpleQueryDto queryDto)
        {
            string? searchTerm = queryDto.SearchTerm;
            SimpleSearchCriteria? searchCriteria = queryDto.SearchCriteria;
            bool? displayAll = queryDto.All;

            var query = _dbContext.ProductionFacilities.AsNoTracking();

            if (searchTerm != null)
            {
                if (searchCriteria == SimpleSearchCriteria.Name)
                {
                    query = query.Where(i => i.Name.ToLower().Contains(searchTerm.ToLower()));
                }
                else
                {
                    query = query.Where(i => i.Id == int.Parse(searchTerm));
                }
            }

            if (!displayAll ?? true)
            {
                query = query.Where(i => i.IsActive);
            }

            IList<ProductionFacility> facilities = await query.ToListAsync();
            return _mapper.Map<IList<ProductionFacilityDto>>(facilities);
        }

        public async Task<ProductionFacilityDto> UpdateAsync(int id, ProductionFacilityInputDto dto)
        {
            ProductionFacility? facility = await _dbContext.ProductionFacilities.FindAsync(id);
            if (facility == null)
            {
                throw new EntityNotFoundException();
            }

            _mapper.Map(dto, facility);

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<ProductionFacilityDto>(facility);
        }
    }
}

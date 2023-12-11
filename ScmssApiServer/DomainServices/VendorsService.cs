using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.DomainServices
{
    public class VendorsService : IVendorsService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public VendorsService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CompanyDto> Add(CompanyInputDto dto)
        {
            var vendor = _mapper.Map<Vendor>(dto);
            _dbContext.Add(vendor);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<CompanyDto>(vendor);
        }

        public async Task<CompanyDto?> Get(int id)
        {
            Vendor? vendor = await _dbContext.Vendors.FindAsync(id);
            return _mapper.Map<CompanyDto?>(vendor);
        }

        public async Task<IList<CompanyDto>> GetMany(SimpleQueryDto queryDto)
        {
            string? searchTerm = queryDto.SearchTerm;
            SimpleSearchCriteria? searchCriteria = queryDto.SearchCriteria;
            bool? displayAll = queryDto.All;

            var query = _dbContext.Vendors.AsQueryable();

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

            IList<Vendor> vendors = await query.ToListAsync();
            return _mapper.Map<IList<CompanyDto>>(vendors);
        }

        public async Task<CompanyDto> Update(int id, CompanyInputDto dto)
        {
            Vendor? vendor = await _dbContext.Vendors.FindAsync(id);
            if (vendor == null)
            {
                throw new EntityNotFoundException();
            }

            _mapper.Map(dto, vendor);

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<CompanyDto>(vendor);
        }
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.DomainServices
{
    public class CustomersService : ICustomersService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public CustomersService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CompanyDto> AddAsync(CompanyInputDto dto)
        {
            var customer = _mapper.Map<Customer>(dto);
            _dbContext.Add(customer);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<CompanyDto>(customer);
        }

        public async Task<CompanyDto?> GetAsync(int id)
        {
            Customer? customer = await _dbContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
            return _mapper.Map<CompanyDto?>(customer);
        }

        public async Task<IList<CompanyDto>> GetManyAsync(SimpleQueryDto dto)
        {
            string? searchTerm = dto.SearchTerm?.ToLower();
            SimpleSearchCriteria? searchCriteria = dto.SearchCriteria;
            bool? displayAll = dto.All;

            var query = _dbContext.Customers.AsNoTracking();

            if (searchTerm != null)
            {
                if (searchCriteria == SimpleSearchCriteria.Name)
                {
                    query = query.Where(i => i.Name.ToLower().Contains(searchTerm));
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

            IList<Customer> customers = await query.OrderBy(i => i.Id).ToListAsync();
            return _mapper.Map<IList<CompanyDto>>(customers);
        }

        public async Task<CompanyDto> UpdateAsync(int id, CompanyInputDto dto)
        {
            Customer? customer = await _dbContext.Customers.FindAsync(id);
            if (customer == null)
            {
                throw new EntityNotFoundException();
            }

            _mapper.Map(dto, customer);

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<CompanyDto>(customer);
        }
    }
}

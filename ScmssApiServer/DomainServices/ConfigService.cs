using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.DomainServices
{
    public class ConfigService : IConfigService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ConfigService(IMapper mapper, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Config> GetAsync()
        {
            return await _dbContext.Config.AsNoTracking().SingleAsync();
        }

        public async Task<Config> SetAsync(ConfigInputDto dto)
        {
            var config = await _dbContext.Config.SingleAsync();
            _mapper.Map(dto, config);
            await _dbContext.SaveChangesAsync();
            return config;
        }
    }
}

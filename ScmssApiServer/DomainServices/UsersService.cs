using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.Exceptions;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.IServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.DomainServices
{
    public class UsersService : IUsersService
    {
        private readonly AppDbContext _dbContext;
        private readonly IFileHostService _fileHostService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UsersService(AppDbContext dbContext,
                            IMapper mapper,
                            IFileHostService fileHostService,
                            UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _fileHostService = fileHostService;
            _userManager = userManager;
        }

        public async Task ChangePasswordAsync(string id, UserPasswordChangeDto dto)
        {
            User? user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new EntityNotFoundException();
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(
                user,
                dto.CurrentPassword,
                dto.NewPassword);

            if (!result.Succeeded)
            {
                throw new IdentityException(result);
            }
        }

        public async Task<UserDto> CreateAsync(UserCreateDto dto)
        {
            if (dto.ProductionFacilityId != null)
            {
                bool isFacilityActive = (await _dbContext.ProductionFacilities
                    .Where(i => i.Id == dto.ProductionFacilityId)
                    .Where(i => i.IsActive)
                    .CountAsync()) > 0;
                if (!isFacilityActive)
                {
                    throw new InvalidDomainOperationException("Production facility not found.");
                }
            }

            var user = _mapper.Map<User>(dto);

            IdentityResult result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                throw new IdentityException(result);
            }

            result = await _userManager.AddToRolesAsync(user, dto.Roles);
            if (!result.Succeeded)
            {
                throw new IdentityException(result);
            }

            return await GetUserDtoAsync(user);
        }

        public async Task<UserDto?> GetAsync(string id)
        {
            User? user = await _userManager.Users
                .AsNoTracking()
                .Include(i => i.ProductionFacility)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (user == null)
            {
                return null;
            }

            return await GetUserDtoAsync(user);
        }

        public async Task<IList<UserDto>> GetManyAsync(SimpleQueryDto dto)
        {
            string? searchTerm = dto.SearchTerm?.ToLower();
            SimpleSearchCriteria? searchCriteria = dto.SearchCriteria;
            bool? displayAll = dto.All;

            var query = _userManager.Users.AsNoTracking();

            if (searchTerm != null)
            {
                if (searchCriteria == SimpleSearchCriteria.Name)
                {
                    query = query.Where(i => i.Name.ToLower().Contains(searchTerm));
                }
                else
                {
                    query = query.Where(i => i.Id == searchTerm);
                }
            }

            if (!displayAll ?? true)
            {
                query = query.Where(i => i.IsActive);
            }

            IList<User> users = await query.Include(i => i.ProductionFacility)
                                           .OrderBy(i => i.Id)
                                           .ToListAsync();
            return _mapper.Map<IList<UserDto>>(users);
        }

        public string GenerateProfileImageUploadUrl(string id)
        {
            return _fileHostService.GenerateUploadUrl(User.ImageFolderKey, id);
        }

        public async Task<UserDto> UpdateAsync(string id, UserInputDto dto)
        {
            if (dto.ProductionFacilityId != null)
            {
                bool isFacilityActive = (await _dbContext.ProductionFacilities
                    .Where(i => i.Id == dto.ProductionFacilityId)
                    .Where(i => i.IsActive)
                    .CountAsync()) > 0;
                if (!isFacilityActive)
                {
                    throw new InvalidDomainOperationException("Production facility not found.");
                }
            }

            User? user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new EntityNotFoundException();
            }

            _mapper.Map(dto, user);

            IdentityResult result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new IdentityException(result);
            }

            IList<string> roles = await _userManager.GetRolesAsync(user);

            result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                throw new IdentityException(result);
            }

            result = await _userManager.AddToRolesAsync(user, dto.Roles);
            if (!result.Succeeded)
            {
                throw new IdentityException(result);
            }

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = dto.Roles;
            return userDto;
        }

        private async Task<UserDto> GetUserDtoAsync(User user)
        {
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = await _userManager.GetRolesAsync(user);
            return userDto;
        }
    }
}

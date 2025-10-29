using AutoMapper;
using SPSS.BusinessObject.Dto.Role;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Constants;
using SPSS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SPSS.Service.Services.Implementations;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRoleRepository _roleRepo;
    private readonly IUserRepository _userRepo; 

    public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
 
        _roleRepo = _unitOfWork.GetRepository<IRoleRepository>();
        _userRepo = _unitOfWork.GetRepository<IUserRepository>();
    }

    public async Task<RoleDto> GetByIdAsync(Guid id)
    {
        var role = await _roleRepo.GetByIdAsync(id); 
        if (role == null)
            throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Role.NotFound, id));
        return _mapper.Map<RoleDto>(role);
    }

    public async Task<PagedResponse<RoleDto>> GetPagedAsync(int pageNumber, int pageSize)
    {
        var (roles, totalCount) = await _roleRepo.GetPagedAsync(pageNumber, pageSize, null); 

        var roleDtos = _mapper.Map<IEnumerable<RoleDto>>(roles);
        return new PagedResponse<RoleDto>(roleDtos, totalCount, pageNumber, pageSize);
    }

    public async Task<RoleDto> CreateAsync(RoleForCreationDto? roleForCreationDto)
    {
        if (roleForCreationDto is null)
            throw new ArgumentNullException(nameof(roleForCreationDto), ExceptionMessageConstants.Role.RoleDataNull);

        var nameExists = await _roleRepo.ExistsAsync(r => r.RoleName == roleForCreationDto.RoleName);
        if (nameExists)
            throw new InvalidOperationException(string.Format(ExceptionMessageConstants.Role.RoleNameAlreadyExists, roleForCreationDto.RoleName));

        var role = _mapper.Map<Role>(roleForCreationDto);
        role.RoleId = Guid.NewGuid();

        _roleRepo.Add(role);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<RoleDto>(role);
    }

    public async Task<RoleDto> UpdateAsync(Guid roleId, RoleForUpdateDto roleForUpdateDto)
    {
        if (roleForUpdateDto is null)
            throw new ArgumentNullException(nameof(roleForUpdateDto), ExceptionMessageConstants.Role.RoleDataNull);

        var role = await _roleRepo.GetByIdAsync(roleId); 
        if (role == null)
            throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Role.NotFound, roleId));

        var nameExists = await _roleRepo.ExistsAsync(r => r.RoleName == roleForUpdateDto.RoleName && r.RoleId != roleId);
        if (nameExists)
            throw new InvalidOperationException(string.Format(ExceptionMessageConstants.Role.RoleNameAlreadyExists, roleForUpdateDto.RoleName));

        _mapper.Map(roleForUpdateDto, role);
        _roleRepo.Update(role); 
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<RoleDto>(role);
    }

    public async Task DeleteAsync(Guid id)
    {
        var role = await _roleRepo.GetByIdAsync(id); 
        if (role == null)
            throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Role.NotFound, id));

        var isUsed = await _userRepo.ExistsAsync(u => u.RoleId == id && !u.IsDeleted);
        if (isUsed)
            throw new InvalidOperationException(ExceptionMessageConstants.Role.InUseByUsers);

        _roleRepo.Delete(role); 
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<RoleDto> GetByNameAsync(string roleName)
    {
        var role = await _roleRepo.GetRoleByNameAsync(roleName);
        if (role == null)
            throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Role.NotFoundByName, roleName));

        return _mapper.Map<RoleDto>(role);
    }
}
using AutoMapper;
using SPSS.BusinessObject.Dto.Role;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Responses;

// --- Giả định bạn có các Interface Repository sau ---
// using Repositories.Interface; 
// public interface IRoleRepository : IRepositoryBase<Role, Guid> 
// {
//     Task<Role?> GetRoleByNameAsync(string roleName);
// }
// ---------------------------------------------------

namespace SPSS.Service.Services.Implementations;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<RoleDto> GetByIdAsync(Guid id)
    {
        var role = await _unitOfWork.GetRepository<IRoleRepository>().GetByIdAsync(id);
        if (role == null)
            throw new KeyNotFoundException($"Role with ID {id} not found.");
        return _mapper.Map<RoleDto>(role);
    }

    public async Task<PagedResponse<RoleDto>> GetPagedAsync(int pageNumber, int pageSize)
    {
        var (roles, totalCount) = await _unitOfWork.GetRepository<IRoleRepository>().GetPagedAsync(pageNumber, pageSize, null);

        // roleDtos là kiểu IEnumerable<RoleDto>
        var roleDtos = _mapper.Map<IEnumerable<RoleDto>>(roles);

        // Gọi thẳng constructor, nó sẽ tự xử lý .ToList().AsReadOnly()
        return new PagedResponse<RoleDto>(
            roleDtos,
            totalCount,
            pageNumber,
            pageSize
        );
    }

    public async Task<RoleDto> CreateAsync(RoleForCreationDto? roleForCreationDto)
    {
        if (roleForCreationDto is null)
            throw new ArgumentNullException(nameof(roleForCreationDto), "Role data cannot be null.");

        var role = _mapper.Map<Role>(roleForCreationDto);
        role.RoleId = Guid.NewGuid();

        _unitOfWork.GetRepository<IRoleRepository>().Add(role);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<RoleDto>(role);
    }

    public async Task<RoleDto> UpdateAsync(Guid roleId, RoleForUpdateDto roleForUpdateDto)
    {
        if (roleForUpdateDto is null)
            throw new ArgumentNullException(nameof(roleForUpdateDto), "Role data cannot be null.");

        var role = await _unitOfWork.GetRepository<IRoleRepository>().GetByIdAsync(roleId);
        if (role == null)
            throw new KeyNotFoundException($"Role with ID {roleId} not found.");

        _mapper.Map(roleForUpdateDto, role);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<RoleDto>(role);
    }


    public async Task DeleteAsync(Guid id)
    {
        var role = await _unitOfWork.GetRepository<IRoleRepository>().GetByIdAsync(id);
        if (role == null)
            throw new KeyNotFoundException($"Role with ID {id} not found.");

        _unitOfWork.GetRepository<IRoleRepository>().Delete(role);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<RoleDto> GetByNameAsync(string roleName)
    {
        var role = await _unitOfWork.GetRepository<IRoleRepository>().GetRoleByNameAsync(roleName);
        if (role == null)
            throw new KeyNotFoundException($"Role {roleName} không tồn tại");

        return _mapper.Map<RoleDto>(role);
    }
}
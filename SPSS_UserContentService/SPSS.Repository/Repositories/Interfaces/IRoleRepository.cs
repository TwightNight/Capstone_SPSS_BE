using SPSS.BusinessObject.Models;
using SPSS.Shared.Base.Interfaces;

namespace SPSS.Repository.Repositories.Interfaces;

public interface IRoleRepository : IRepositoryBase<Role, Guid>
{
    Task<Role?> GetRoleByNameAsync(string roleName);
}
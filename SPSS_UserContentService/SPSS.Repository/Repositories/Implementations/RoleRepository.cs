using Microsoft.EntityFrameworkCore;
using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class RoleRepository : RepositoryBase<Role, Guid>, IRoleRepository
{
    public RoleRepository(UserDBContext context) : base(context)
    {
        
    }
    public async Task<Role?> GetRoleByNameAsync(string roleName)
    {
        return await Entities
            .FirstOrDefaultAsync(r => r.RoleName == roleName);
    }
}
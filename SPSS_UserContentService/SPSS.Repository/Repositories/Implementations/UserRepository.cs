using Microsoft.EntityFrameworkCore;
using SPSS.BusinessObject.Context; 
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class UserRepository : RepositoryBase<User, Guid>, IUserRepository
{
    private IQueryable<User> ActiveUsers => Entities.Where(u => !u.IsDeleted);

    public UserRepository(UserDBContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await ActiveUsers
            .FirstOrDefaultAsync(u => u.EmailAddress == email, cancellationToken);
    }

    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await ActiveUsers
            .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
    }
}
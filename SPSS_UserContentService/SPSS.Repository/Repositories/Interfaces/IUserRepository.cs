using SPSS.BusinessObject.Models;
using SPSS.Shared.Base.Interfaces;

namespace SPSS.Repository.Repositories.Interfaces;

public interface IUserRepository : IRepositoryBase<User, Guid>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
}
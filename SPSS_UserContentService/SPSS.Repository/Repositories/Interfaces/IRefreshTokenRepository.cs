using SPSS.BusinessObject.Models;
using SPSS.Shared.Base.Interfaces;

namespace SPSS.Repository.Repositories.Interfaces;

public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken, Guid>
{
    Task<RefreshToken?> GetByTokenAsync(string token);

    Task<IReadOnlyCollection<RefreshToken>> GetByUserIdAsync(Guid userId);
}
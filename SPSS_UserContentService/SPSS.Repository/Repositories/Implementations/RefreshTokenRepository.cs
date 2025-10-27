using Microsoft.EntityFrameworkCore;
using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class RefreshTokenRepository : RepositoryBase<RefreshToken, Guid>, IRefreshTokenRepository
{
    public RefreshTokenRepository(UserDBContext context) : base(context)
    {
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        var refreshToken = await Entities 
            .FirstOrDefaultAsync(u => u.Token == token);

        return refreshToken;
    }

    public async Task<IReadOnlyCollection<RefreshToken>> GetByUserIdAsync(Guid userId)
    {
        var refreshTokens = await Entities 
            .Where(rt => rt.UserId == userId)
            .ToListAsync();

        return refreshTokens;
    }
}
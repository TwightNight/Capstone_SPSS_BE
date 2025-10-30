using Microsoft.EntityFrameworkCore;
using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Repository.Repositories.Implementations;

public class EmailVerificationRepository : RepositoryBase<EmailVerification, Guid>, IEmailVerificationRepository
{
    public EmailVerificationRepository(UserDBContext context) : base(context) { }

    public async Task<EmailVerification?> GetLatestByEmailAsync(string email)
    {
        return await Entities
            .Where(e => e.Email == email)
            .OrderByDescending(e => e.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task RevokeAllByUserAsync(Guid userId)
    {
        var list = await Entities.Where(e => e.UserId == userId && !e.IsRevoked && !e.IsUsed).ToListAsync();
        foreach (var item in list)
        {
            item.IsRevoked = true;
        }
        UpdateRange(list);
    }
}

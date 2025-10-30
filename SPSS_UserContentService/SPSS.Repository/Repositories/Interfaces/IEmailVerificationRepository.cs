using SPSS.BusinessObject.Models;
using SPSS.Shared.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Repository.Repositories.Interfaces;

public interface IEmailVerificationRepository : IRepositoryBase<EmailVerification, Guid>
{
    Task<EmailVerification?> GetLatestByEmailAsync(string email);
    Task RevokeAllByUserAsync(Guid userId);
}

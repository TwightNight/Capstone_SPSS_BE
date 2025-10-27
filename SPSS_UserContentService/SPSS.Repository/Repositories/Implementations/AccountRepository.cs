using Microsoft.EntityFrameworkCore;
using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class AccountRepository : RepositoryBase<User, Guid>, IAccountRepository
{
    public AccountRepository(UserDBContext context) : base(context)
    {
    }
}

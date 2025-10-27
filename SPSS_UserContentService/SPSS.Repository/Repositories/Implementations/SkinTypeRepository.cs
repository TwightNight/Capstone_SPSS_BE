using Microsoft.EntityFrameworkCore;
using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class SkinTypeRepository : RepositoryBase<SkinType, Guid>, ISkinTypeRepository
{
    public SkinTypeRepository(UserDBContext context) : base(context)
    {
    }
}

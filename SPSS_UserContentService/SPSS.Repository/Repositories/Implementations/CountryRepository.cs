using Microsoft.EntityFrameworkCore;
using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class CountryRepository : RepositoryBase<Country, Guid> , ICountryRepository
{
    public CountryRepository(UserDBContext context) : base(context)
    {
    }
}

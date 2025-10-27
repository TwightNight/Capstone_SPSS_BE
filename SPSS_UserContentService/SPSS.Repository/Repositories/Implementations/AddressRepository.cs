using Microsoft.EntityFrameworkCore;
using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class AddressRepository : RepositoryBase<Address, Guid>, IAddressRepository
{
    public AddressRepository(UserDBContext context) : base(context)
    {
    }
}

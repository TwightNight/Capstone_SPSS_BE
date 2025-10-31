using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class StatusChangeRepository : RepositoryBase<StatusChange, Guid>, IStatusChangeRepository
{
    public StatusChangeRepository(ProductOrderDBContext context) : base(context)
    {
    }
}
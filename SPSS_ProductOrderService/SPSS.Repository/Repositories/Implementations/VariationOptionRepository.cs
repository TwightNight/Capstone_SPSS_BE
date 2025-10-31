using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class VariationOptionRepository : RepositoryBase<VariationOption, Guid>, IVariationOptionRepository
{
    public VariationOptionRepository(ProductOrderDBContext context) : base(context)
    {
    }
}
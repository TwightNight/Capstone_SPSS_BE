using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class VariationRepository : RepositoryBase<Variation, Guid>, IVariationRepository
{
    public VariationRepository(ProductOrderDBContext context) : base(context)
    {
    }
}
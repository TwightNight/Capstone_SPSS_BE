using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class ProductForSkinConditionRepository : RepositoryBase<ProductForSkinCondition, Guid>, IProductForSkinConditionRepository
{
    public ProductForSkinConditionRepository(ProductOrderDBContext context) : base(context)
    {
    }
}
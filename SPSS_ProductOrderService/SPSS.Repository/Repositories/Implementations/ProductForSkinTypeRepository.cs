using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class ProductForSkinTypeRepository : RepositoryBase<ProductForSkinType, Guid>, IProductForSkinTypeRepository
{
    public ProductForSkinTypeRepository(ProductOrderDBContext context) : base(context)
    {
    }
}
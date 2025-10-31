using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class ProductCategoryRepository : RepositoryBase<ProductCategory, Guid>, IProductCategoryRepository
{
    public ProductCategoryRepository(ProductOrderDBContext context) : base(context)
    {
    }
}
using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class ProductImageRepository : RepositoryBase<ProductImage, Guid>, IProductImageRepository
{
    public ProductImageRepository(ProductOrderDBContext context) : base(context)
    {
    }
}
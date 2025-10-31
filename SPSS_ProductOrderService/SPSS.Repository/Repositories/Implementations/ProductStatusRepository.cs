using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class ProductStatusRepository : RepositoryBase<ProductStatus, Guid>, IProductStatusRepository
{
    public ProductStatusRepository(ProductOrderDBContext context) : base(context)
    {
    }
}   
using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;
public class ProductConfigurationRepository : RepositoryBase<ProductConfiguration, Guid>, IProductConfigurationRepository
{
    public ProductConfigurationRepository(ProductOrderDBContext context) : base(context)
    {
    }
}
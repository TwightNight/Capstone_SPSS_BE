using SPSS.BusinessObject.Models;
using SPSS.Shared.Base.Interfaces;

namespace SPSS.Repository.Repositories.Interfaces;

public interface IProductRepository : IRepositoryBase<Product, Guid>
{
}
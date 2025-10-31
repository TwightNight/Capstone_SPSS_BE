using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class OrderDetailRepository : RepositoryBase<OrderDetail, Guid>, IOrderDetailRepository
{
    public OrderDetailRepository(ProductOrderDBContext context) : base(context)
    {
    }
}
using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class CartItemRepository : RepositoryBase<CartItem, Guid>, ICartItemRepository
{
    public CartItemRepository(ProductOrderDBContext context) : base(context)
    {
    }
}
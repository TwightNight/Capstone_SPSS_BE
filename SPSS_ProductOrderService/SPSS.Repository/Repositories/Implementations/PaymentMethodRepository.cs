using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class PaymentMethodRepository : RepositoryBase<PaymentMethod, Guid>, IPaymentMethodRepository
{
    public PaymentMethodRepository(ProductOrderDBContext context) : base(context)
    {
    }
}
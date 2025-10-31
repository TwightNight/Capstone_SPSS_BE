using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class VoucherRepository : RepositoryBase<Voucher, Guid>, IVoucherRepository
{
    public VoucherRepository(ProductOrderDBContext context) : base(context)
    {
    }
}
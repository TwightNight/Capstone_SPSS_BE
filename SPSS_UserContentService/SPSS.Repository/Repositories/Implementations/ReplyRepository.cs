using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class ReplyRepository : RepositoryBase<Reply, Guid>, IReplyRepository
{
    public ReplyRepository(UserDBContext context) : base(context)
    {
    }
}

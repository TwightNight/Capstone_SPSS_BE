using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

public class ReviewImageRepository : RepositoryBase<ReviewImage, Guid>, IReviewImageRepository
{
    public ReviewImageRepository(UserDBContext context) : base(context)
    {
    }
}

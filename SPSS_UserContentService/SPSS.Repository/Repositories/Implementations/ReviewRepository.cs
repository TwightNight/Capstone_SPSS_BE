using Microsoft.EntityFrameworkCore;
using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base.Implementations;

namespace SPSS.Repository.Repositories.Implementations;

class ReviewRepository : RepositoryBase<Review, Guid>, IReviewRepository
{
    public ReviewRepository(UserDBContext context) : base(context)
    {
    }
}

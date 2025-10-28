using SPSS.BusinessObject.Dto.Blog;
using SPSS.Shared.Responses;

namespace SPSS.Service.Services.Interfaces;

public interface IBlogService
{
    Task<BlogWithDetailDto> GetByIdAsync(Guid id);
    Task<PagedResponse<BlogDto>> GetPagedAsync(int pageNumber, int pageSize);
    Task<BlogDto> CreateBlogAsync(BlogForCreationDto blogDto, Guid userId);
    Task<BlogDto> UpdateBlogAsync(Guid blogId, BlogForUpdateDto blogDto, Guid userId);
    Task<bool> DeleteAsync(Guid id, Guid userId);
}
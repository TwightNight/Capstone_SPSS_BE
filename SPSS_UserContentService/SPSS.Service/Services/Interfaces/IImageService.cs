using Microsoft.AspNetCore.Http;

namespace SPSS.Service.Services.Interfaces;

/// <summary>
/// Interface trừu tượng cho việc xử lý upload/delete hình ảnh.
/// </summary>
public interface IImageService
{
    /// <summary>
    /// Upload một file ảnh và trả về URL.
    /// </summary>
    /// <param name="file">File ảnh từ request.</param>
    /// <returns>URL công khai của ảnh đã upload.</returns>
    Task<string> UploadImageAsync(IFormFile file);

    /// <summary>
    /// Xóa một file ảnh dựa trên URL của nó.
    /// </summary>
    /// <param name="imageUrl">URL công khai của ảnh cần xóa.</param>
    Task DeleteImageAsync(string imageUrl);
}

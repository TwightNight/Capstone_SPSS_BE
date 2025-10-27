using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Services.Interface;
using SPSS.Service.Services.Interfaces;

namespace SPSS.Service.Services.Implementations;
/// <summary>
/// Implementation của IImageService sử dụng Firebase Storage.
/// </summary>
public class ImageService : IImageService
{
    private readonly string _bucketName;
    private readonly ILogger<ImageService> _logger;
    private static readonly string[] _validImageExtensions =
        { "jpg", "jpeg", "png", "gif", "bmp", "tiff", "webp" };

    public ImageService(IConfiguration configuration, ILogger<ImageService> logger)
    {
        // Inject config và logger
        _bucketName = configuration["Firebase:BucketName"] ??
                      throw new ArgumentNullException("Firebase:BucketName not found in configuration.");
        _logger = logger;
    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File cannot be null or empty.");
        }

        // 1. Xác thực file (logic từ ManageFirebaseImageService)
        var mediaType = Path.GetExtension(file.FileName).TrimStart('.').ToLower();
        if (string.IsNullOrEmpty(mediaType) || !_validImageExtensions.Contains(mediaType))
        {
            throw new Exception("Only image files are allowed.");
        }

        // 2. Tạo tên file duy nhất
        var newFileName = $"{Guid.NewGuid()}.{mediaType}";
        var storagePath = $"SPSS/Product-Image/{newFileName}"; // Bạn có thể đổi folder nếu muốn

        // 3. Upload
        using var stream = file.OpenReadStream();
        var task = new FirebaseStorage(_bucketName)
            .Child(storagePath)
            .PutAsync(stream);

        var downloadUrl = await task;
        return downloadUrl;
    }

    public async Task DeleteImageAsync(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl)) return;

        try
        {
            var storagePath = GetStoragePathFromUrl(imageUrl);
            await new FirebaseStorage(_bucketName)
                .Child(storagePath)
                .DeleteAsync();
        }
        catch (Exception ex)
        {
            // QUAN TRỌNG: Chỉ log lỗi, không ném exception.
            // Việc không xóa được avatar cũ không nên làm hỏng toàn bộ request.
            _logger.LogWarning(ex, "Failed to delete old image from Firebase: {ImageUrl}", imageUrl);
        }
    }

    private string GetStoragePathFromUrl(string fileUrl)
    {
        var uri = new Uri(fileUrl);
        var segments = uri.Segments;

        // URL Firebase có dạng: .../v0/b/BUCKET/o/PATH...
        // segments[4] là "o/"
        // segments[5] trở đi là đường dẫn file
        if (segments.Length > 5 && segments[4].Equals("o/", StringComparison.OrdinalIgnoreCase))
        {
            string storagePath = string.Join("", segments.Skip(5));
            int queryIndex = storagePath.IndexOf('?');
            if (queryIndex >= 0)
            {
                storagePath = storagePath.Substring(0, queryIndex);
            }
            return Uri.UnescapeDataString(storagePath);
        }

        throw new ArgumentException("Invalid Firebase file URL format.");
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.Blog;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Responses;
using System.Security;
using System.Security.Claims;

namespace SPSS.Api.Controllers;

[ApiController]
[Route("api/blogs")]
public class BlogController : ControllerBase
{
    private readonly IBlogService _blogService;
    private readonly ILogger<BlogController> _logger;

    public BlogController(IBlogService blogService, ILogger<BlogController> logger)
    {
        _blogService = blogService;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResponse<BlogDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBlogs([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var response = await _blogService.GetPagedAsync(pageNumber, pageSize);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy danh sách blog phân trang.");
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BlogWithDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBlogById(Guid id)
    {
        try
        {
            var blogDetail = await _blogService.GetByIdAsync(id);
            return Ok(blogDetail);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy chi tiết blog {BlogId}", id);
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    [HttpPost]
    [Authorize] // Yêu cầu đăng nhập
    [ProducesResponseType(typeof(BlogDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBlog([FromBody] BlogForCreationDto blogDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = GetUserIdFromClaims();
            var createdBlog = await _blogService.CreateBlogAsync(blogDto, userId);
            return CreatedAtAction(nameof(GetBlogById), new { id = createdBlog.Id }, createdBlog);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo blog mới.");
            return StatusCode(500, "Lỗi hệ thống khi tạo blog.");
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize] // Yêu cầu đăng nhập
    [ProducesResponseType(typeof(BlogDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBlog(Guid id, [FromBody] BlogForUpdateDto blogDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = GetUserIdFromClaims();
            var updatedBlog = await _blogService.UpdateBlogAsync(id, blogDto, userId);
            return Ok(updatedBlog);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (SecurityException ex) // Bắt lỗi 403 từ Service
        {
            return Forbid(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật blog {BlogId}", id);
            return StatusCode(500, "Lỗi hệ thống khi cập nhật blog.");
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize] // Yêu cầu đăng nhập
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBlog(Guid id)
    {
        try
        {
            var userId = GetUserIdFromClaims();
            await _blogService.DeleteAsync(id, userId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (SecurityException ex) // Bắt lỗi 403 từ Service
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa blog {BlogId}", id);
            return StatusCode(500, "Lỗi hệ thống khi xóa blog.");
        }
    }

    private Guid GetUserIdFromClaims()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            _logger.LogWarning("Không thể tìm thấy hoặc phân tích UserId từ claims.");
            throw new SecurityException("Thông tin người dùng không hợp lệ.");
        }

        return userId;
    }
}
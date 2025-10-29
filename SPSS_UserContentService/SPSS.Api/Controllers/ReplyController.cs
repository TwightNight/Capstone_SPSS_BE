using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.Reply;
using SPSS.Service.Interfaces; // Đổi thành SPSS.Service.Services.Interfaces
using System.Security.Claims;
using System.Security;

namespace SPSS.Api.Controllers;

[ApiController]
[Route("api/replies")]
//[Authorize] // BẮT BUỘC: Mọi hành động reply đều cần đăng nhập
public class ReplyController : ControllerBase
{
    private readonly IReplyService _replyService;
    private readonly ILogger<ReplyController> _logger;

    public ReplyController(IReplyService replyService, ILogger<ReplyController> logger)
    {
        _replyService = replyService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReplyDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateReply([FromBody] ReplyForCreationDto replyDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = GetUserIdFromClaims();
            var createdReply = await _replyService.CreateAsync(userId, replyDto);

            // Trả về 201 Created
            // (Giả sử không có endpoint GetById cho Reply, nên trả về object)
            return CreatedAtAction(null, new { id = createdReply.Id }, createdReply);
        }
        catch (ArgumentException ex) // Bắt lỗi ReviewId không tồn tại
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex) // Bắt lỗi "FailedToSave"
        {
            _logger.LogError(ex, "Lỗi khi tạo reply cho user {UserId}", GetUserIdFromClaims());
            return StatusCode(500, "Lỗi hệ thống khi tạo reply.");
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ReplyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateReply(Guid id, [FromBody] ReplyForUpdateDto replyDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = GetUserIdFromClaims();
            var updatedReply = await _replyService.UpdateAsync(userId, replyDto, id);
            return Ok(updatedReply);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (SecurityException ex) // Bắt lỗi "NotOwner"
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật reply {ReplyId} cho user {UserId}", id, GetUserIdFromClaims());
            return StatusCode(500, "Lỗi hệ thống khi cập nhật reply.");
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReply(Guid id)
    {
        try
        {
            var userId = GetUserIdFromClaims();
            await _replyService.DeleteAsync(userId, id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (SecurityException ex) // Bắt lỗi "NotOwner"
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa reply {ReplyId} cho user {UserId}", id, GetUserIdFromClaims());
            return StatusCode(500, "Lỗi hệ thống khi xóa reply.");
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
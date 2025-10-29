using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.ChatHistory;
using SPSS.Service.Services.Interfaces;
using System.Security.Claims;
using System.Security;

namespace SPSS.Api.Controllers;

[ApiController]
[Route("api/chathistory")]
//[Authorize] // BẮT BUỘC: Toàn bộ lịch sử chat phải được bảo vệ
public class ChatHistoryController : ControllerBase
{
    private readonly IChatHistoryService _chatService;
    private readonly ILogger<ChatHistoryController> _logger;

    public ChatHistoryController(IChatHistoryService chatService, ILogger<ChatHistoryController> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }

    [HttpGet("sessions")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyRecentSessionIds([FromQuery] int maxSessions = 10)
    {
        try
        {
            var userId = GetUserIdFromClaims();
            var sessionIds = await _chatService.GetRecentSessionsIdsAsync(userId, maxSessions);
            // Service đã trả về [] nếu rỗng, nên ta chỉ cần trả về Ok()
            return Ok(sessionIds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy danh sách session IDs cho user {UserId}", GetUserIdFromClaims());
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    [HttpGet("{sessionId}")]
    [ProducesResponseType(typeof(IEnumerable<ChatHistoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyChatSession(string sessionId)
    {
        try
        {
            var userId = GetUserIdFromClaims();
            // Dùng hàm bảo mật, kiểm tra cả UserId và SessionId
            var sessionHistory = await _chatService.GetChatHistoryByUserIdAndSessionIdAsync(userId, sessionId);
            return Ok(sessionHistory);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy lịch sử session {SessionId} cho user {UserId}", sessionId, GetUserIdFromClaims());
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    [HttpGet("recent-messages")]
    [ProducesResponseType(typeof(IEnumerable<ChatHistoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyRecentMessages([FromQuery] int limit = 100)
    {
        try
        {
            var userId = GetUserIdFromClaims();
            var messages = await _chatService.GetChatHistoryByUserIdAsync(userId, limit);
            return Ok(messages);
        }
        catch (KeyNotFoundException ex)
        {
            // Service ném lỗi này nếu không tìm thấy, trả về 404 là hợp lý
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy {Limit} tin nhắn gần nhất cho user {UserId}", limit, GetUserIdFromClaims());
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(ChatHistoryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SaveChatMessage([FromBody] ChatHistoryForCreationDto chatDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = GetUserIdFromClaims();

            // --- LOGIC BẢO MẬT QUAN TRỌNG ---
            // Ghi đè UserId trong DTO bằng UserId từ Token
            // để ngăn mạo danh.
            chatDto.UserId = userId;
            // --- KẾT THÚC LOGIC BẢO MẬT ---

            // Giả sử DTO chưa có timestamp, service nên tự gán
            // (Nếu DTO có, bạn có thể gán ở đây: chatDto.Timestamp = DateTime.UtcNow;)

            var createdMessage = await _chatService.SaveChatMessageAsync(chatDto);

            // Trả về 201 Created
            return CreatedAtAction(nameof(GetMyChatSession), new { sessionId = createdMessage.SessionId }, createdMessage);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex) // Bắt lỗi "FailedToSave" từ Service
        {
            _logger.LogError(ex, "Lỗi khi lưu tin nhắn chat cho user {UserId}", GetUserIdFromClaims());
            return StatusCode(500, "Lỗi hệ thống khi lưu tin nhắn.");
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.ChatHistory;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Exceptions;
using SPSS.Shared.Responses;
using System;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SPSS.Api.Controllers;

[ApiController]
[Route("api/chathistory")]
[Authorize]
public class ChatHistoryController : ControllerBase
{
    private readonly IChatHistoryService _chatService;
    private readonly ILogger<ChatHistoryController> _logger;

    public ChatHistoryController(IChatHistoryService chatService, ILogger<ChatHistoryController> logger)
    {
        _chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("sessions")]
    public async Task<IActionResult> GetMyRecentSessionIds([FromQuery] int maxSessions = 10)
    {
        var userId = GetUserIdFromClaims();
        var sessionIds = await _chatService.GetRecentSessionsIdsAsync(userId, maxSessions);
        return Ok(ApiResponse.Ok(sessionIds, "Recent session IDs retrieved successfully."));
    }

    [HttpGet("{sessionId}")]
    public async Task<IActionResult> GetMyChatSession(string sessionId)
    {
        var userId = GetUserIdFromClaims();
        var sessionHistory = await _chatService.GetChatHistoryByUserIdAndSessionIdAsync(userId, sessionId);
        return Ok(ApiResponse.Ok(sessionHistory, "Chat session history retrieved successfully."));
    }

    [HttpGet("recent-messages")]
    public async Task<IActionResult> GetMyRecentMessages([FromQuery] int limit = 100)
    {
        var userId = GetUserIdFromClaims();
        var messages = await _chatService.GetChatHistoryByUserIdAsync(userId, limit);
        return Ok(ApiResponse.Ok(messages, "Recent chat messages retrieved successfully."));
    }

    [HttpPost]
    public async Task<IActionResult> SaveChatMessage([FromBody] ChatHistoryForCreationDto chatDto)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            throw new ValidationException(string.Join(" | ", errorMessages));
        }
        var userId = GetUserIdFromClaims();
        chatDto.UserId = userId;

        var createdMessage = await _chatService.SaveChatMessageAsync(chatDto);
        var response = ApiResponse.Ok(createdMessage, "Chat message saved successfully.");
        return CreatedAtAction(nameof(GetMyChatSession), new { sessionId = createdMessage.SessionId }, response);
    }

    private Guid GetUserIdFromClaims()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            _logger.LogWarning("Could not find or parse UserId from claims.");
            throw new SecurityException("User identifier is missing or invalid in the security token.");
        }
        return userId;
    }
}
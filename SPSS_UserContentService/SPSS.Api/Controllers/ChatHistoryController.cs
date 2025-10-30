using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.ChatHistory;
using SPSS.Service.Services.Interfaces;
using System.Security.Claims;
using System.Security;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using SPSS.Shared.Errors;
using SPSS.Shared.Exceptions;

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
        return Ok(sessionIds);
    }

    [HttpGet("{sessionId}")]
    public async Task<IActionResult> GetMyChatSession(string sessionId)
    {
        var userId = GetUserIdFromClaims();
        var sessionHistory = await _chatService.GetChatHistoryByUserIdAndSessionIdAsync(userId, sessionId);
        return Ok(sessionHistory);
    }

    [HttpGet("recent-messages")]
    public async Task<IActionResult> GetMyRecentMessages([FromQuery] int limit = 100)
    {
        var userId = GetUserIdFromClaims();
        var messages = await _chatService.GetChatHistoryByUserIdAsync(userId, limit);
        return Ok(messages);
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
        return CreatedAtAction(nameof(GetMyChatSession), new { sessionId = createdMessage.SessionId }, createdMessage);
    }

    private Guid GetUserIdFromClaims()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            _logger.LogWarning("Không thể tìm thấy hoặc phân tích UserId từ claims.");
            throw new SecurityException("Thông tin người dùng không hợp lệ trong token.");
        }
        return userId;
    }
}
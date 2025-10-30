using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.Reply;
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
[Route("api/replies")]
[Authorize]
public class ReplyController : ControllerBase
{
    private readonly IReplyService _replyService;
    private readonly ILogger<ReplyController> _logger;

    public ReplyController(IReplyService replyService, ILogger<ReplyController> logger)
    {
        _replyService = replyService ?? throw new ArgumentNullException(nameof(replyService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost]
    public async Task<IActionResult> CreateReply([FromBody] ReplyForCreationDto replyDto)
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
        var createdReply = await _replyService.CreateAsync(userId, replyDto);
        var response = ApiResponse.Ok(createdReply, "Reply created successfully.");
        return CreatedAtAction(null, new { id = createdReply.Id }, response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateReply(Guid id, [FromBody] ReplyForUpdateDto replyDto)
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
        var updatedReply = await _replyService.UpdateAsync(userId, replyDto, id);
        return Ok(ApiResponse.Ok(updatedReply, "Reply updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteReply(Guid id)
    {
        var userId = GetUserIdFromClaims();
        await _replyService.DeleteAsync(userId, id);
        return Ok(ApiResponse.Ok<object>(null, "Reply deleted successfully."));
    }

    private Guid GetUserIdFromClaims()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            throw new SecurityException("User identifier is missing or invalid in the security token.");
        }
        return userId;
    }
}
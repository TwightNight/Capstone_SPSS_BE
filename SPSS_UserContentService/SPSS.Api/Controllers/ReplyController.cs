using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.Reply;
using SPSS.Service.Services.Interfaces; 
using SPSS.Shared.Errors;
using System;
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
    [ProducesResponseType(typeof(ReplyDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateReply([FromBody] ReplyForCreationDto replyDto)
    {
        var userId = GetUserIdFromClaims();
        var createdReply = await _replyService.CreateAsync(userId, replyDto);
        return CreatedAtAction(null, new { id = createdReply.Id }, createdReply);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ReplyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateReply(Guid id, [FromBody] ReplyForUpdateDto replyDto)
    {
        var userId = GetUserIdFromClaims();
        var updatedReply = await _replyService.UpdateAsync(userId, replyDto, id);
        return Ok(updatedReply);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReply(Guid id)
    {
        var userId = GetUserIdFromClaims();
        await _replyService.DeleteAsync(userId, id);
        return NoContent();
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
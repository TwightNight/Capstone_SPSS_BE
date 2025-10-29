using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.Reply;
using SPSS.Service.Interfaces;
using SPSS.Service.Services.Interfaces; // Đã sửa namespace
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateReply([FromBody] ReplyForCreationDto replyDto)
    {
        try
        {
            var userId = GetUserIdFromClaims();
            var createdReply = await _replyService.CreateAsync(userId, replyDto);
            return CreatedAtAction(null, new { id = createdReply.Id }, createdReply);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ReplyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateReply(Guid id, [FromBody] ReplyForUpdateDto replyDto)
    {
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
        catch (SecurityException)
        {
            return Forbid();
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
        catch (SecurityException)
        {
            return Forbid();
        }
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
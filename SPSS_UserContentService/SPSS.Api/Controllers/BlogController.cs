using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.Blog;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Errors;
using SPSS.Shared.Exceptions;
using SPSS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SPSS.Api.Controllers;

[ApiController]
[Route("api/blogs")]
public class BlogController : ControllerBase
{
    private readonly IBlogService _blogService;
    private readonly ILogger<BlogController> _logger;

    public BlogController(IBlogService blogService, ILogger<BlogController> logger)
    {
        _blogService = blogService ?? throw new ArgumentNullException(nameof(blogService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetBlogs([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var response = await _blogService.GetPagedAsync(pageNumber, pageSize);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBlogById(Guid id)
    {
        var blogDetail = await _blogService.GetByIdAsync(id);
        return Ok(blogDetail);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateBlog([FromBody] BlogForCreationDto blogDto)
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
        var createdBlog = await _blogService.CreateBlogAsync(blogDto, userId);
        return CreatedAtAction(nameof(GetBlogById), new { id = createdBlog.Id }, createdBlog);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateBlog(Guid id, [FromBody] BlogForUpdateDto blogDto)
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
        var updatedBlog = await _blogService.UpdateBlogAsync(id, blogDto, userId);
        return Ok(updatedBlog);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteBlog(Guid id)
    {
        var userId = GetUserIdFromClaims();
        await _blogService.DeleteAsync(id, userId);
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
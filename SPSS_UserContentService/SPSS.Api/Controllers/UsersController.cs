using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.User;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Exceptions;
using SPSS.Shared.Responses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SPSS.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsersPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var response = await _userService.GetPagedAsync(pageNumber, pageSize);
        return Ok(ApiResponse.Ok(response, "Users retrieved successfully."));
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        return Ok(ApiResponse.Ok(user, "User retrieved successfully."));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUser([FromBody] UserForCreationDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            throw new ValidationException(string.Join(" | ", errorMessages));
        }
        var createdUser = await _userService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetUserById), new { id = createdUser.UserId }, ApiResponse.Ok(createdUser, "User created successfully."));
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserForUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            throw new ValidationException(string.Join(" | ", errorMessages));
        }
        var updatedUser = await _userService.UpdateAsync(id, dto);
        return Ok(ApiResponse.Ok(updatedUser, "User updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        await _userService.DeleteAsync(id);
        return Ok(ApiResponse.Ok<object>(null, "User deleted successfully."));
    }
}
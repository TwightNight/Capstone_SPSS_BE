using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.Address;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Responses;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic; // Cần cho KeyNotFoundException
using Microsoft.Extensions.Logging; // Cần cho ILogger
using System.Security; // Cần cho SecurityException

namespace SPSS.Api.Controllers;

[ApiController]
[Route("api/addresses")] // Đặt route chuẩn
//[Authorize] // BẮT BUỘC: Đảm bảo người dùng đã đăng nhập
public class AddressesController : ControllerBase
{
    private readonly IAddressService _addressService;
    private readonly ILogger<AddressesController> _logger;

    // Tiêm (Inject) IAddressService và ILogger
    public AddressesController(IAddressService addressService, ILogger<AddressesController> logger)
    {
        _addressService = addressService;
        _logger = logger;
    }

    /// <summary>
    /// Lấy danh sách địa chỉ (phân trang) của người dùng hiện tại.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<AddressDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyAddresses([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var userId = GetUserIdFromClaims();
            var pagedResponse = await _addressService.GetByUserIdPagedAsync(userId, pageNumber, pageSize);
            return Ok(pagedResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy danh sách địa chỉ cho người dùng.");
            return StatusCode(500, "Đã xảy ra lỗi hệ thống. Vui lòng thử lại sau.");
        }
    }

    /// <summary>
    /// Tạo một địa chỉ mới cho người dùng hiện tại.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AddressDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAddress([FromBody] AddressForCreationDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = GetUserIdFromClaims();
            var createdAddress = await _addressService.CreateAsync(createDto, userId);

            // Trả về 201 Created cùng với object vừa tạo.
            // Lý tưởng nhất là có một route "GetAddressById" để trỏ tới.
            return CreatedAtAction(nameof(GetMyAddresses), new { id = createdAddress.Id }, createdAddress);
        }
        catch (ArgumentException ex) // Bắt lỗi nghiệp vụ (ví dụ: CountryId không tồn tại)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo địa chỉ mới.");
            return StatusCode(500, "Đã xảy ra lỗi hệ thống khi tạo địa chỉ.");
        }
    }

    /// <summary>
    /// Cập nhật một địa chỉ đã có của người dùng hiện tại.
    /// </summary>
    [HttpPut("{addressId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAddress(Guid addressId, [FromBody] AddressForUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = GetUserIdFromClaims();
            await _addressService.UpdateAsync(addressId, updateDto, userId);
            return NoContent(); // 204: Thành công, không cần trả về nội dung
        }
        catch (KeyNotFoundException ex) // Bắt lỗi không tìm thấy từ Service
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex) // Bắt lỗi nghiệp vụ
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật địa chỉ {AddressId}.", addressId);
            return StatusCode(500, "Đã xảy ra lỗi hệ thống khi cập nhật địa chỉ.");
        }
    }

    /// <summary>
    /// Xóa một địa chỉ của người dùng hiện tại.
    /// </summary>
    [HttpDelete("{addressId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAddress(Guid addressId)
    {
        try
        {
            var userId = GetUserIdFromClaims();
            await _addressService.DeleteAsync(addressId, userId);
            return NoContent(); // 204: Thành công
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex) // Bắt lỗi "Không thể xóa địa chỉ mặc định"
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa địa chỉ {AddressId}.", addressId);
            return StatusCode(500, "Đã xảy ra lỗi hệ thống khi xóa địa chỉ.");
        }
    }

    /// <summary>
    /// Đặt một địa chỉ làm mặc định cho người dùng hiện tại.
    /// </summary>
    [HttpPatch("{addressId:guid}/set-default")] // Dùng PATCH cho hành động cập nhật một phần
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetDefaultAddress(Guid addressId)
    {
        try
        {
            var userId = GetUserIdFromClaims();
            await _addressService.SetAsDefaultAsync(addressId, userId);
            return NoContent(); // 204: Thành công
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi đặt địa chỉ mặc định {AddressId}.", addressId);
            return StatusCode(500, "Đã xảy ra lỗi hệ thống.");
        }
    }

    // --- PRIVATE HELPER ---

    /// <summary>
    /// (Hàm private) Lấy UserId của người dùng từ token (Claims).
    /// Đây là mấu chốt để đảm bảo bảo mật.
    /// </summary>
    private Guid GetUserIdFromClaims()
    {
        // Tên Claim phụ thuộc vào cách bạn cấu hình JWT
        // Thường là ClaimTypes.NameIdentifier hoặc một claim tùy chỉnh như "uid"
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            // Lỗi này không nên xảy ra nếu có [Authorize], nhưng vẫn check
            _logger.LogWarning("Không thể tìm thấy hoặc phân tích UserId từ claims.");
            throw new SecurityException("Thông tin người dùng không hợp lệ.");
        }

        return userId;
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.Country;
using SPSS.Service.Services.Interfaces;
using System.Security;

namespace SPSS.Api.Controllers
{
    [ApiController]
    [Route("api/countries")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<CountryController> _logger;

        public CountryController(ICountryService countryService, ILogger<CountryController> logger)
        {
            _countryService = countryService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<CountryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCountries()
        {
            try
            {
                var countries = await _countryService.GetAllAsync();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách tất cả quốc gia.");
                return StatusCode(500, "Lỗi hệ thống.");
            }
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CountryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var country = await _countryService.GetByIdAsync(id);
                return Ok(country);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tìm quốc gia có ID {CountryId}", id);
                return StatusCode(500, "Lỗi hệ thống.");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CountryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateCountry([FromBody] CountryForCreationDto countryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdCountry = await _countryService.CreateAsync(countryDto);
                return CreatedAtAction(nameof(GetById), new { id = createdCountry.Id }, createdCountry);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex) // Bắt lỗi trùng code hoặc trùng tên
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo quốc gia mới với tên {CountryName}", countryDto.CountryName);
                return StatusCode(500, "Lỗi hệ thống khi tạo quốc gia.");
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CountryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] CountryForUpdateDto countryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedCountry = await _countryService.UpdateAsync(id, countryDto);
                return Ok(updatedCountry);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex) // Bắt lỗi trùng code hoặc trùng tên
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật quốc gia {CountryId}", id);
                return StatusCode(500, "Lỗi hệ thống khi cập nhật quốc gia.");
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            try
            {
                await _countryService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex) // Bắt lỗi "Country in use"
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa quốc gia {CountryId}", id);
                return StatusCode(500, "Lỗi hệ thống khi xóa quốc gia.");
            }
        }
    }
}
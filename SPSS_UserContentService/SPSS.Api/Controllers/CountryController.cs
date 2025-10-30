using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.Country;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Exceptions;
using SPSS.Shared.Responses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SPSS.Api.Controllers;

[ApiController]
[Route("api/countries")]
public class CountryController : ControllerBase
{
    private readonly ICountryService _countryService;
    private readonly ILogger<CountryController> _logger;

    public CountryController(ICountryService countryService, ILogger<CountryController> logger)
    {
        _countryService = countryService ?? throw new ArgumentNullException(nameof(countryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCountries()
    {
        var countries = await _countryService.GetAllAsync();
        return Ok(ApiResponse.Ok(countries, "Countries retrieved successfully."));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var country = await _countryService.GetByIdAsync(id);
        return Ok(ApiResponse.Ok(country, "Country retrieved successfully."));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateCountry([FromBody] CountryForCreationDto countryDto)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            throw new ValidationException(string.Join(" | ", errorMessages));
        }
        var createdCountry = await _countryService.CreateAsync(countryDto);
        return CreatedAtAction(nameof(GetById), new { id = createdCountry.Id }, ApiResponse.Ok(createdCountry, "Country created successfully."));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCountry(int id, [FromBody] CountryForUpdateDto countryDto)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            throw new ValidationException(string.Join(" | ", errorMessages));
        }
        var updatedCountry = await _countryService.UpdateAsync(id, countryDto);
        return Ok(ApiResponse.Ok(updatedCountry, "Country updated successfully."));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        await _countryService.DeleteAsync(id);
        return Ok(ApiResponse.Ok<object>(null, "Country deleted successfully."));
    }
}
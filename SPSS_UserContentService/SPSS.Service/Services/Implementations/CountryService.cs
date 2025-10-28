using AutoMapper;

using SPSS.BusinessObject.Dto.Country;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Service.Services.Interfaces;

namespace SPSS.Service.Services.Implementations;

public class CountryService : ICountryService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICountryRepository _countryRepo;
    private readonly IAddressRepository _addressRepo; // Cần để kiểm tra khi xóa

    public CountryService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;

        // Sử dụng đúng UoW pattern
        _countryRepo = _unitOfWork.GetRepository<ICountryRepository>();
        _addressRepo = _unitOfWork.GetRepository<IAddressRepository>();
    }

    public async Task<CountryDto> GetByIdAsync(int id)
    {
        // Dùng repo
        var country = await _countryRepo.GetByIdAsync(id);
        if (country == null)
            throw new KeyNotFoundException($"Country with ID {id} not found.");

        return _mapper.Map<CountryDto>(country);
    }

    public async Task<IEnumerable<CountryDto>> GetAllAsync()
    {
        // Dùng repo
        var countries = await _countryRepo.GetAsync();
        return _mapper.Map<IEnumerable<CountryDto>>(countries);
    }

    public async Task<CountryDto> CreateAsync(CountryForCreationDto countryForCreationDto)
    {
        if (countryForCreationDto == null)
            throw new ArgumentNullException(nameof(countryForCreationDto), "Country data cannot be null.");

        // Logic nghiệp vụ: Kiểm tra trùng lặp
        var codeExists = await _countryRepo.ExistsAsync(c => c.CountryCode == countryForCreationDto.CountryCode);
        if (codeExists)
            throw new InvalidOperationException($"Country code '{countryForCreationDto.CountryCode}' already exists.");

        var nameExists = await _countryRepo.ExistsAsync(c => c.CountryName == countryForCreationDto.CountryName);
        if (nameExists)
            throw new InvalidOperationException($"Country name '{countryForCreationDto.CountryName}' already exists.");

        var country = _mapper.Map<Country>(countryForCreationDto);

        _countryRepo.Add(country);
        await _unitOfWork.SaveChangesAsync();

        // EF sẽ tự động điền 'country.Id' sau khi SaveChanges
        return _mapper.Map<CountryDto>(country);
    }

    public async Task<CountryDto> UpdateAsync(int countryId, CountryForUpdateDto countryForUpdateDto)
    {
        if (countryForUpdateDto == null)
            throw new ArgumentNullException(nameof(countryForUpdateDto), "Country data cannot be null.");

        var country = await _countryRepo.GetByIdAsync(countryId);
        if (country == null)
            throw new KeyNotFoundException($"Country with ID {countryId} not found.");

        // Logic nghiệp vụ: Kiểm tra trùng lặp (ngoại trừ chính nó)
        var codeExists = await _countryRepo.ExistsAsync(c => c.CountryCode == countryForUpdateDto.CountryCode && c.Id != countryId);
        if (codeExists)
            throw new InvalidOperationException($"Country code '{countryForUpdateDto.CountryCode}' already exists.");

        var nameExists = await _countryRepo.ExistsAsync(c => c.CountryName == countryForUpdateDto.CountryName && c.Id != countryId);
        if (nameExists)
            throw new InvalidOperationException($"Country name '{countryForUpdateDto.CountryName}' already exists.");

        // Map DTO vào entity đã tồn tại
        _mapper.Map(countryForUpdateDto, country);

        _countryRepo.Update(country);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CountryDto>(country);
    }

    public async Task DeleteAsync(int id)
    {
        var country = await _countryRepo.GetByIdAsync(id);
        if (country == null)
            throw new KeyNotFoundException($"Country with ID {id} not found.");

        // Logic nghiệp vụ (QUAN TRỌNG): Kiểm tra xem quốc gia có đang được sử dụng không
        var isUsed = await _addressRepo.ExistsAsync(a => a.CountryId == id && !a.IsDeleted);
        if (isUsed)
            throw new InvalidOperationException("Cannot delete country. It is currently in use by one or more addresses.");

        // Chỉ xóa khi không được sử dụng
        _countryRepo.Delete(country);
        await _unitOfWork.SaveChangesAsync();
    }
}

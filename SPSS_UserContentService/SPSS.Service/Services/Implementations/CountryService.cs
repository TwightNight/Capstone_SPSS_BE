using AutoMapper;
using SPSS.BusinessObject.Dto.Country;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
		var country = await _countryRepo.GetByIdAsync(id);
		if (country == null)
			throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Country.NotFound, id));

		return _mapper.Map<CountryDto>(country);
	}

	public async Task<IEnumerable<CountryDto>> GetAllAsync()
	{
		var countries = await _countryRepo.GetAsync();
		return _mapper.Map<IEnumerable<CountryDto>>(countries);
	}

	public async Task<CountryDto> CreateAsync(CountryForCreationDto countryForCreationDto)
	{
		if (countryForCreationDto == null)
			throw new ArgumentNullException(nameof(countryForCreationDto), ExceptionMessageConstants.Country.CountryDataNull);

		// Logic nghiệp vụ: Kiểm tra trùng lặp
		var codeExists = await _countryRepo.ExistsAsync(c => c.CountryCode == countryForCreationDto.CountryCode);
		if (codeExists)
			throw new InvalidOperationException(string.Format(ExceptionMessageConstants.Country.CodeAlreadyExists, countryForCreationDto.CountryCode));

		var nameExists = await _countryRepo.ExistsAsync(c => c.CountryName == countryForCreationDto.CountryName);
		if (nameExists)
			throw new InvalidOperationException(string.Format(ExceptionMessageConstants.Country.NameAlreadyExists, countryForCreationDto.CountryName));

		var country = _mapper.Map<Country>(countryForCreationDto);

		_countryRepo.Add(country);
		await _unitOfWork.SaveChangesAsync();

		return _mapper.Map<CountryDto>(country);
	}

	public async Task<CountryDto> UpdateAsync(int countryId, CountryForUpdateDto countryForUpdateDto)
	{
		if (countryForUpdateDto == null)
			throw new ArgumentNullException(nameof(countryForUpdateDto), ExceptionMessageConstants.Country.CountryDataNull);

		var country = await _countryRepo.GetByIdAsync(countryId);
		if (country == null)
			throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Country.NotFound, countryId));

		// Kiểm tra trùng lặp (ngoại trừ chính nó)
		var codeExists = await _countryRepo.ExistsAsync(c => c.CountryCode == countryForUpdateDto.CountryCode && c.Id != countryId);
		if (codeExists)
			throw new InvalidOperationException(string.Format(ExceptionMessageConstants.Country.CodeAlreadyExists, countryForUpdateDto.CountryCode));

		var nameExists = await _countryRepo.ExistsAsync(c => c.CountryName == countryForUpdateDto.CountryName && c.Id != countryId);
		if (nameExists)
			throw new InvalidOperationException(string.Format(ExceptionMessageConstants.Country.NameAlreadyExists, countryForUpdateDto.CountryName));

		_mapper.Map(countryForUpdateDto, country);

		_countryRepo.Update(country);
		await _unitOfWork.SaveChangesAsync();

		return _mapper.Map<CountryDto>(country);
	}

	public async Task DeleteAsync(int id)
	{
		var country = await _countryRepo.GetByIdAsync(id);
		if (country == null)
			throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Country.NotFound, id));

		var isUsed = await _addressRepo.ExistsAsync(a => a.CountryId == id && !a.IsDeleted);
		if (isUsed)
			throw new InvalidOperationException(ExceptionMessageConstants.Country.InUseByAddresses);

		_countryRepo.Delete(country);
		await _unitOfWork.SaveChangesAsync();
	}
}

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SPSS.BusinessObject.Dto.Address;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Responses;
using SPSS.Shared.Constants;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SPSS.Service.Services.Implementations
{
	public class AddressService : IAddressService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		// Khai báo các repository cụ thể
		private readonly IAddressRepository _addressRepo;
		private readonly ICountryRepository _countryRepo;
		private readonly IUserRepository _userRepo;

		public AddressService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

			// Lấy các repository từ UnitOfWork
			_addressRepo = _unitOfWork.GetRepository<IAddressRepository>();
			_countryRepo = _unitOfWork.GetRepository<ICountryRepository>();
			_userRepo = _unitOfWork.GetRepository<IUserRepository>();
		}

		public async Task<PagedResponse<AddressDto>> GetByUserIdPagedAsync(Guid userId, int pageNumber, int pageSize)
		{
			var query = _addressRepo.Entities
				.Where(a => a.UserId == userId && !a.IsDeleted);

			int totalCount = await query.CountAsync();

			var addressDtos = await query
				.OrderBy(a => a.CreatedTime)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return new PagedResponse<AddressDto>(
				addressDtos,
				totalCount,
				pageNumber,
				pageSize
			);
		}

		public async Task<AddressDto> CreateAsync(AddressForCreationDto? addressForCreationDto, Guid userId)
		{
			if (addressForCreationDto is null)
				throw new ArgumentNullException(nameof(addressForCreationDto), ExceptionMessageConstants.Address.AddressDataNull);

			var countryExists = await _countryRepo.ExistsAsync(c => c.Id == addressForCreationDto.CountryId);
			if (!countryExists)
				throw new ArgumentException(string.Format(ExceptionMessageConstants.Country.NotFound, addressForCreationDto.CountryId));

			var userExists = await _userRepo.ExistsAsync(u => u.UserId == userId);
			if (!userExists)
				throw new ArgumentException(string.Format(ExceptionMessageConstants.User.NotFound, userId));

			var address = _mapper.Map<Address>(addressForCreationDto);

			address.Id = Guid.NewGuid();
			address.UserId = userId;
			address.CreatedTime = DateTimeOffset.UtcNow;
			address.CreatedBy = userId.ToString();
			address.LastUpdatedTime = DateTimeOffset.UtcNow;
			address.LastUpdatedBy = userId.ToString();
			address.IsDeleted = false;

			if (address.IsDefault)
			{
				await _unitOfWork.BeginTransactionAsync();
				try
				{
					await UnsetDefaultAddressesAsync(userId);
					_addressRepo.Add(address);
					await _unitOfWork.CommitTransactionAsync();
				}
				catch (Exception ex)
				{
					await _unitOfWork.RollbackTransactionAsync();
					throw new Exception(string.Format(ExceptionMessageConstants.Address.FailedToCreate, ex.Message), ex);
				}
			}
			else
			{
				_addressRepo.Add(address);
				await _unitOfWork.SaveChangesAsync();
			}

			var savedAddress = await _addressRepo.GetSingleAsync(
				predicate: a => a.Id == address.Id,
				include: q => q.Include(a => a.Country)
			);

			return _mapper.Map<AddressDto>(savedAddress);
		}

		public async Task<bool> UpdateAsync(Guid addressId, AddressForUpdateDto addressForUpdateDto, Guid userId)
		{
			if (addressForUpdateDto is null)
				throw new ArgumentNullException(nameof(addressForUpdateDto), ExceptionMessageConstants.Address.AddressDataNull);

			var countryExists = await _countryRepo.ExistsAsync(c => c.Id == addressForUpdateDto.CountryId);
			if (!countryExists)
				throw new ArgumentException(string.Format(ExceptionMessageConstants.Country.NotFound, addressForUpdateDto.CountryId));

			var address = await _addressRepo.GetByIdAsync(addressId);
			if (address == null || address.UserId != userId)
				throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Address.NotFound, addressId));

			_mapper.Map(addressForUpdateDto, address);

			address.LastUpdatedTime = DateTimeOffset.UtcNow;
			address.LastUpdatedBy = userId.ToString();

			_addressRepo.Update(address);
			await _unitOfWork.SaveChangesAsync();
			return true;
		}

		public async Task<bool> SetAsDefaultAsync(Guid addressId, Guid userId)
		{
			var addressToSetDefault = await _addressRepo.GetSingleAsync(
				predicate: a => a.Id == addressId && a.UserId == userId && !a.IsDeleted);

			if (addressToSetDefault == null)
				throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Address.NotFoundForUser, addressId));

			if (addressToSetDefault.IsDefault)
				return true;

			await _unitOfWork.BeginTransactionAsync();
			try
			{
				await UnsetDefaultAddressesAsync(userId, addressToSetDefault.Id);

				addressToSetDefault.IsDefault = true;
				addressToSetDefault.LastUpdatedBy = userId.ToString();
				addressToSetDefault.LastUpdatedTime = DateTimeOffset.UtcNow;
				_addressRepo.Update(addressToSetDefault);

				await _unitOfWork.CommitTransactionAsync();
				return true;
			}
			catch (Exception ex)
			{
				await _unitOfWork.RollbackTransactionAsync();
				throw new Exception(string.Format(ExceptionMessageConstants.Address.FailedToSetAsDefault, ex.Message), ex);
			}
		}

		public async Task<bool> DeleteAsync(Guid id, Guid userId)
		{
			var address = await _addressRepo.GetByIdAsync(id);
			if (address == null || address.UserId != userId)
				throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Address.NotFound, id));

			if (address.IsDefault)
				throw new InvalidOperationException(ExceptionMessageConstants.Address.CannotDeleteDefault);

			address.DeletedBy = userId.ToString();
			address.DeletedTime = DateTimeOffset.UtcNow;
			address.IsDeleted = true;

			_addressRepo.Update(address);
			await _unitOfWork.SaveChangesAsync();
			return true;
		}

		/// <summary>
		/// (Hàm private) Bỏ đặt mặc định tất cả các địa chỉ của người dùng.
		/// </summary>
		private async Task UnsetDefaultAddressesAsync(Guid userId, Guid? excludeAddressId = null)
		{
			var userAddresses = await _addressRepo.GetAsync(
				filter: a => a.UserId == userId && a.IsDefault && !a.IsDeleted
			);

			foreach (var address in userAddresses)
			{
				if (address.Id != excludeAddressId)
				{
					address.IsDefault = false;
					address.LastUpdatedBy = userId.ToString();
					address.LastUpdatedTime = DateTimeOffset.UtcNow;
					_addressRepo.Update(address);
				}
			}
			// Do not call SaveChanges here; caller manages persistence (transaction or direct save).
		}
	}
}

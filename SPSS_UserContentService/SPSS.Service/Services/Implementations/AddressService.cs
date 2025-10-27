using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SPSS.BusinessObject.Dto.Address;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Responses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SPSS.Service.Services.Implementations;

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
        _unitOfWork = unitOfWork;
        _mapper = mapper;

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

        // --- ĐÂY LÀ DÒNG ĐÃ SỬA ---
        // Chúng ta gọi constructor với 4 tham số thay vì dùng object initializer
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
            throw new ArgumentNullException(nameof(addressForCreationDto), "Address data cannot be null.");

        // Dùng _countryRepo.ExistsAsync
        var countryExists = await _countryRepo.ExistsAsync(c => c.Id == addressForCreationDto.CountryId);
        if (!countryExists)
            throw new ArgumentException($"Country with ID {addressForCreationDto.CountryId} does not exist.");

        // Dùng _userRepo.ExistsAsync
        var userExists = await _userRepo.ExistsAsync(u => u.UserId == userId);
        if (!userExists)
            throw new ArgumentException($"User with ID {userId} does not exist.");

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
                _addressRepo.Add(address); // Dùng _addressRepo
                await _unitOfWork.CommitTransactionAsync(); // Commit sẽ gọi SaveChangesAsync()
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Failed to create address: {ex.Message}", ex);
            }
        }
        else
        {
            _addressRepo.Add(address); // Dùng _addressRepo
            await _unitOfWork.SaveChangesAsync();
        }

        // Dùng _addressRepo.GetSingleAsync để lấy dữ liệu
        var savedAddress = await _addressRepo.GetSingleAsync(
            predicate: a => a.Id == address.Id,
            include: q => q.Include(a => a.Country)
        );

        return _mapper.Map<AddressDto>(savedAddress);
    }

    public async Task<bool> UpdateAsync(Guid addressId, AddressForUpdateDto addressForUpdateDto, Guid userId)
    {
        if (addressForUpdateDto is null)
            throw new ArgumentNullException(nameof(addressForUpdateDto), "Address data cannot be null.");

        // Dùng _countryRepo.ExistsAsync
        var countryExists = await _countryRepo.ExistsAsync(c => c.Id == addressForUpdateDto.CountryId);
        if (!countryExists)
            throw new ArgumentException($"Country with ID {addressForUpdateDto.CountryId} does not exist.");

        // Dùng _addressRepo.GetByIdAsync
        var address = await _addressRepo.GetByIdAsync(addressId);
        if (address == null || address.UserId != userId)
            throw new KeyNotFoundException($"Address with ID {addressId} not found.");

        _mapper.Map(addressForUpdateDto, address);

        address.LastUpdatedTime = DateTimeOffset.UtcNow;
        address.LastUpdatedBy = userId.ToString();

        _addressRepo.Update(address); // Dùng _addressRepo
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SetAsDefaultAsync(Guid addressId, Guid userId)
    {
        // Dùng _addressRepo.GetSingleAsync
        var addressToSetDefault = await _addressRepo.GetSingleAsync(
            predicate: a => a.Id == addressId && a.UserId == userId && !a.IsDeleted);

        if (addressToSetDefault == null)
            throw new KeyNotFoundException($"Address with ID {addressId} not found for the current user.");

        if (addressToSetDefault.IsDefault)
            return true;

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await UnsetDefaultAddressesAsync(userId, addressToSetDefault.Id);

            addressToSetDefault.IsDefault = true;
            addressToSetDefault.LastUpdatedBy = userId.ToString();
            addressToSetDefault.LastUpdatedTime = DateTimeOffset.UtcNow;
            _addressRepo.Update(addressToSetDefault); // Dùng _addressRepo

            await _unitOfWork.CommitTransactionAsync();
            return true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw new Exception($"Failed to set address as default: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        // Dùng _addressRepo.GetByIdAsync
        var address = await _addressRepo.GetByIdAsync(id);
        if (address == null || address.UserId != userId)
            throw new KeyNotFoundException($"Address with ID {id} not found.");

        if (address.IsDefault)
            throw new InvalidOperationException("Cannot delete the default address. Set another address as default first.");

        address.DeletedBy = userId.ToString();
        address.DeletedTime = DateTimeOffset.UtcNow;
        address.IsDeleted = true;

        _addressRepo.Update(address); // Dùng _addressRepo
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// (Hàm private) Bỏ đặt mặc định tất cả các địa chỉ của người dùng.
    /// </summary>
    private async Task UnsetDefaultAddressesAsync(Guid userId, Guid? excludeAddressId = null)
    {
        // Dùng _addressRepo.GetAsync
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
                _addressRepo.Update(address); // Dùng _addressRepo
            }
        }
        // Không SaveChanges() ở đây, để cho hàm gọi (trong transaction) tự quản lý.
    }
}
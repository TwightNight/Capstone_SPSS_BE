using SPSS.BusinessObject.Dto.Address;
using SPSS.Shared.Responses;

namespace SPSS.Service.Services.Interfaces;

public interface IAddressService
{
    Task<PagedResponse<AddressDto>> GetByUserIdPagedAsync(Guid userId, int pageNumber, int pageSize);
    Task<AddressDto> CreateAsync(AddressForCreationDto? addressForCreationDto, Guid userId);
    Task<bool> UpdateAsync(Guid addressId, AddressForUpdateDto addressForUpdateDto, Guid userId);
    Task<bool> DeleteAsync(Guid id, Guid userId);
    Task<bool> SetAsDefaultAsync(Guid addressId, Guid userId);
}
using SPSS.BusinessObject.Dto.Account;
using SPSS.BusinessObject.Dto.User;
using SPSS.Shared.Responses;

namespace SPSS.Service.Services.Interfaces;

public interface IUserService
{
    #region Admin Management Methods
    Task<UserDto> GetByIdAsync(Guid id);
    Task<UserDto> GetByEmailAsync(string email);
    Task<UserDto> GetByUserNameAsync(string userName);
    Task<PagedResponse<UserDto>> GetPagedAsync(int pageNumber, int pageSize);
    Task<UserDto> CreateAsync(UserForCreationDto? userForCreationDto);
    Task<UserDto> UpdateAsync(Guid userId, UserForUpdateDto userForUpdateDto);
    Task DeleteAsync(Guid id);
    #endregion

    #region Validation Methods
    Task<bool> CheckUserNameExistsAsync(string userName, Guid? excludeUserId = null);
    Task<bool> CheckEmailExistsAsync(string email, Guid? excludeUserId = null);
    Task<bool> CheckPhoneNumberExistsAsync(string phoneNumber, Guid? excludeUserId = null);
    #endregion

    #region User Profile Methods
    Task<AccountDto> GetAccountInfoAsync(Guid userId);
    Task<AccountDto> UpdateAccountInfoAsync(Guid userId, AccountForUpdateDto accountUpdateDto);
    Task<string> UpdateAvatarAsync(Guid userId, string newAvatarUrl);
    #endregion
}
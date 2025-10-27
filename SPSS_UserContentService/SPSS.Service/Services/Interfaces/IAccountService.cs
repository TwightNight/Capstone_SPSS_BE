using Microsoft.AspNetCore.Http;
using SPSS.BusinessObject.Dto.Account;

namespace SPSS.Service.Services.Interfaces;

public interface IAccountService
{
    Task<AccountDto> GetAccountInfoAsync(Guid userId);
    Task<AccountDto> UpdateAccountInfoAsync(Guid userId, AccountForUpdateDto accountUpdateDto);
    Task<string> UpdateAvatarAsync(Guid userId, IFormFile avatarFile);
}

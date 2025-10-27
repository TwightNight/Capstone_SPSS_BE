using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SPSS.BusinessObject.Dto.Account;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Service.Services.Interfaces;

namespace SPSS.Service.Services.Implementations;

public class AccountService : IAccountService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService; // <-- ĐÃ THAY ĐỔI
    private readonly IAccountRepository _accountRepo;
    private readonly IMapper _mapper;

    public AccountService(
        IUnitOfWork unitOfWork,
        IImageService imageService, // <-- ĐÃ THAY ĐỔI
        IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService)); // <-- ĐÃ THAY ĐỔI
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        _accountRepo = _unitOfWork.GetRepository<IAccountRepository>();
    }

    // ... GetAccountInfoAsync và UpdateAccountInfoAsync không thay đổi ...

    public async Task<AccountDto> GetAccountInfoAsync(Guid userId)
    {
        var user = await _accountRepo.GetSingleAsync(
            predicate: u => u.UserId == userId && !u.IsDeleted,
            include: q => q.Include(u => u.SkinType)
        );
        if (user == null)
            throw new KeyNotFoundException($"Không tìm thấy người dùng với ID {userId}.");
        return _mapper.Map<AccountDto>(user);
    }

    public async Task<AccountDto> UpdateAccountInfoAsync(Guid userId, AccountForUpdateDto accountUpdateDto)
    {
        var user = await _accountRepo.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException($"Không tìm thấy người dùng với ID {userId}.");
        _mapper.Map(accountUpdateDto, user);
        user.LastUpdatedBy = userId.ToString();
        user.LastUpdatedTime = DateTimeOffset.UtcNow;
        _accountRepo.Update(user);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<AccountDto>(user);
    }

    // UpdateAvatarAsync đã được cập nhật để dùng IImageService
    public async Task<string> UpdateAvatarAsync(Guid userId, IFormFile avatarFile)
    {
        if (avatarFile == null || avatarFile.Length == 0)
        {
            throw new ArgumentException("Avatar file cannot be null or empty.", nameof(avatarFile));
        }

        var user = await _accountRepo.GetByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"Không tìm thấy người dùng với ID {userId}.");
        }

        var oldAvatarUrl = user.AvatarUrl;

        // 1. Upload ảnh mới bằng service đã refactor
        string newAvatarUrl = await _imageService.UploadImageAsync(avatarFile);

        if (string.IsNullOrEmpty(newAvatarUrl))
        {
            throw new Exception("Lỗi upload avatar và lấy URL.");
        }

        // 2. Cập nhật DB
        user.AvatarUrl = newAvatarUrl;
        user.LastUpdatedBy = userId.ToString();
        user.LastUpdatedTime = DateTimeOffset.UtcNow;
        _accountRepo.Update(user);
        await _unitOfWork.SaveChangesAsync();

        // 3. Xóa ảnh cũ (fire-and-forget, service sẽ tự log lỗi nếu có)
        if (!string.IsNullOrEmpty(oldAvatarUrl))
        {
            await _imageService.DeleteImageAsync(oldAvatarUrl);
        }

        return newAvatarUrl;
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query; // <-- Cần import thêm cái này
using SPSS.BusinessObject.Dto.Account;
using SPSS.BusinessObject.Dto.User;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Responses;
using System.Linq.Expressions;

namespace SPSS.Service.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepo;
    private readonly IRoleRepository _roleRepo;

    // SỬA LỖI: Đổi kiểu trả về từ IQueryable<User> sang IIncludableQueryable<User, object>
    private readonly Func<IQueryable<User>, IIncludableQueryable<User, object>> _includeFullUserDetails =
        q => q.Include(u => u.Role)
              .Include(u => u.SkinType)
              .Include(u => u.SkinCondition);

    // SỬA LỖI: Đổi kiểu trả về từ IQueryable<User> sang IIncludableQueryable<User, object>
    private readonly Func<IQueryable<User>, IIncludableQueryable<User, object>> _includeAccountDetails =
        q => q.Include(u => u.SkinType)
              .Include(u => u.SkinCondition);


    public UserService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;

        _userRepo = _unitOfWork.GetRepository<IUserRepository>();
        _roleRepo = _unitOfWork.GetRepository<IRoleRepository>();
    }

    #region Admin Management Methods

    public async Task<UserDto> GetByIdAsync(Guid id)
    {
        var user = await _userRepo.GetSingleAsync(
            predicate: u => u.UserId == id && !u.IsDeleted,
            include: _includeFullUserDetails // Lỗi CS1503 (dòng 47) đã được sửa
        );

        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found.");

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> GetByEmailAsync(string email)
    {
        var user = await _userRepo.GetSingleAsync(
            predicate: u => u.EmailAddress == email && !u.IsDeleted,
            include: _includeFullUserDetails // Lỗi CS1503 (dòng 60) đã được sửa
        );

        if (user == null)
            throw new KeyNotFoundException($"User with email {email} not found.");

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> GetByUserNameAsync(string userName)
    {
        var user = await _userRepo.GetSingleAsync(
            predicate: u => u.UserName == userName && !u.IsDeleted,
            include: _includeFullUserDetails // Lỗi CS1503 (dòng 73) đã được sửa
        );

        if (user == null)
            throw new KeyNotFoundException($"User with user name {userName} not found.");

        return _mapper.Map<UserDto>(user);
    }

    public async Task<PagedResponse<UserDto>> GetPagedAsync(int pageNumber, int pageSize)
    {
        var (users, totalCount) = await _userRepo.GetPagedAsync(
            pageNumber,
            pageSize,
            filter: u => !u.IsDeleted,
            orderBy: q => q.OrderBy(u => u.UserName),
            include: q => q.Include(u => u.Role)
        // Lưu ý: GetPagedAsync chấp nhận IIncludableQueryable, nên ở đây vẫn OK
        );

        var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

        return new PagedResponse<UserDto>(userDtos, totalCount, pageNumber, pageSize);
    }

    public async Task<UserDto> CreateAsync(UserForCreationDto? userForCreationDto)
    {
        if (userForCreationDto == null)
            throw new ArgumentNullException(nameof(userForCreationDto), "User data cannot be null.");

        if (await CheckEmailExistsAsync(userForCreationDto.EmailAddress))
            throw new InvalidOperationException($"Email {userForCreationDto.EmailAddress} is already in use.");

        if (await CheckUserNameExistsAsync(userForCreationDto.UserName))
            throw new InvalidOperationException($"Username {userForCreationDto.UserName} is already in use.");

        if (await CheckPhoneNumberExistsAsync(userForCreationDto.PhoneNumber))
            throw new InvalidOperationException($"Phone number {userForCreationDto.PhoneNumber} is already in use.");

        var user = _mapper.Map<User>(userForCreationDto);

        user.UserId = Guid.NewGuid();
        user.CreatedBy = "System";
        user.CreatedTime = DateTimeOffset.UtcNow;
        user.LastUpdatedBy = "System";
        user.LastUpdatedTime = DateTimeOffset.UtcNow;
        user.IsDeleted = false;

        _userRepo.Add(user);
        await _unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(user.UserId);
    }

    public async Task<UserDto> UpdateAsync(Guid userId, UserForUpdateDto userForUpdateDto)
    {
        if (userForUpdateDto == null)
            throw new ArgumentNullException(nameof(userForUpdateDto), "User data cannot be null.");

        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null || user.IsDeleted)
            throw new KeyNotFoundException($"User with ID {userId} not found.");

        if (await CheckEmailExistsAsync(userForUpdateDto.EmailAddress, userId))
            throw new InvalidOperationException($"Email {userForUpdateDto.EmailAddress} is already in use.");

        if (await CheckUserNameExistsAsync(userForUpdateDto.UserName, userId))
            throw new InvalidOperationException($"Username {userForUpdateDto.UserName} is already in use.");

        if (await CheckPhoneNumberExistsAsync(userForUpdateDto.PhoneNumber, userId))
            throw new InvalidOperationException($"Phone number {userForUpdateDto.PhoneNumber} is already in use.");

        _mapper.Map(userForUpdateDto, user);

        user.LastUpdatedTime = DateTimeOffset.UtcNow;
        user.LastUpdatedBy = "System";

        _userRepo.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(user.UserId);
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user == null || user.IsDeleted)
            throw new KeyNotFoundException($"User with ID {id} not found.");

        user.IsDeleted = true;
        user.DeletedTime = DateTimeOffset.UtcNow;
        user.DeletedBy = "System";

        _userRepo.Update(user);
        await _unitOfWork.SaveChangesAsync();
    }

    #endregion

    #region Validation Methods

    public async Task<bool> CheckUserNameExistsAsync(string userName, Guid? excludeUserId = null)
    {
        Expression<Func<User, bool>> predicate;
        if (excludeUserId.HasValue)
        {
            predicate = u => u.UserName == userName && u.UserId != excludeUserId.Value && !u.IsDeleted;
        }
        else
        {
            predicate = u => u.UserName == userName && !u.IsDeleted;
        }
        return await _userRepo.ExistsAsync(predicate);
    }

    public async Task<bool> CheckEmailExistsAsync(string email, Guid? excludeUserId = null)
    {
        Expression<Func<User, bool>> predicate;
        if (excludeUserId.HasValue)
        {
            predicate = u => u.EmailAddress == email && u.UserId != excludeUserId.Value && !u.IsDeleted;
        }
        else
        {
            predicate = u => u.EmailAddress == email && !u.IsDeleted;
        }
        return await _userRepo.ExistsAsync(predicate);
    }

    public async Task<bool> CheckPhoneNumberExistsAsync(string phoneNumber, Guid? excludeUserId = null)
    {
        Expression<Func<User, bool>> predicate;
        if (excludeUserId.HasValue)
        {
            predicate = u => u.PhoneNumber == phoneNumber && u.UserId != excludeUserId.Value && !u.IsDeleted;
        }
        else
        {
            predicate = u => u.PhoneNumber == phoneNumber && !u.IsDeleted;
        }
        return await _userRepo.ExistsAsync(predicate);
    }

    #endregion

    #region User Profile Methods

    public async Task<AccountDto> GetAccountInfoAsync(Guid userId)
    {
        var user = await _userRepo.GetSingleAsync(
            predicate: u => u.UserId == userId && !u.IsDeleted,
            include: _includeAccountDetails // Lỗi CS1503 (dòng 223) đã được sửa
        );

        if (user == null)
            throw new KeyNotFoundException($"Không tìm thấy người dùng với ID {userId}.");

        return _mapper.Map<AccountDto>(user);
    }

    public async Task<AccountDto> UpdateAccountInfoAsync(Guid userId, AccountForUpdateDto accountUpdateDto)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException($"Không tìm thấy người dùng với ID {userId}.");

        if (accountUpdateDto.EmailAddress != null &&
            await CheckEmailExistsAsync(accountUpdateDto.EmailAddress, userId))
            throw new InvalidOperationException($"Email {accountUpdateDto.EmailAddress} is already in use.");

        if (accountUpdateDto.UserName != null &&
            await CheckUserNameExistsAsync(accountUpdateDto.UserName, userId))
            throw new InvalidOperationException($"Username {accountUpdateDto.UserName} is already in use.");

        if (accountUpdateDto.PhoneNumber != null &&
            await CheckPhoneNumberExistsAsync(accountUpdateDto.PhoneNumber, userId))
            throw new InvalidOperationException($"Phone number {accountUpdateDto.PhoneNumber} is already in use.");

        _mapper.Map(accountUpdateDto, user);

        user.LastUpdatedBy = userId.ToString();
        user.LastUpdatedTime = DateTimeOffset.UtcNow;

        _userRepo.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return await GetAccountInfoAsync(user.UserId);
    }

    public async Task<string> UpdateAvatarAsync(Guid userId, string newAvatarUrl)
    {
        if (string.IsNullOrEmpty(newAvatarUrl))
        {
            throw new ArgumentException("Avatar URL cannot be null or empty.", nameof(newAvatarUrl));
        }

        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"Không tìm thấy người dùng với ID {userId}.");
        }

        string oldAvatarUrl = user.AvatarUrl;

        user.AvatarUrl = newAvatarUrl;
        user.LastUpdatedBy = userId.ToString();
        user.LastUpdatedTime = DateTimeOffset.UtcNow;

        _userRepo.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return oldAvatarUrl;
    }

    #endregion
}
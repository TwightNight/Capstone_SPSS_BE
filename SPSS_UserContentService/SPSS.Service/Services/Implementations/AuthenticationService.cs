using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Services.Interface;
using SPSS.BusinessObject.Dto.Authentication;
using SPSS.BusinessObject.Dto.Role;
using SPSS.BusinessObject.Dto.User;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Service.Services.Interface;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Helpers;
using System.Text.RegularExpressions;

// --- Giả định bạn có các Interface Repository sau ---
// using Repositories.Interface; 
// public interface IUserRepository : IRepositoryBase<User, Guid> { }
// public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken, Guid> 
// {
//     Task<RefreshToken?> GetByTokenAsync(string token);
// }
// ---------------------------------------------------

namespace SPSS.Service.Services.Implementations;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly IPasswordHasher _passwordHasher;

    public AuthenticationService(IRoleService roleService, IUserService userService, IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _mapper = mapper;
        _userService = userService;
        _roleService = roleService;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthenticationResponse> LoginAsync(LoginRequest loginRequest)
    {
        var usernameOrEmail = loginRequest.UsernameOrEmail;

        var user = await _unitOfWork.GetRepository<IUserRepository>().Entities
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.EmailAddress == usernameOrEmail || u.UserName == usernameOrEmail);

        if (user == null || user.IsDeleted)
            throw new UnauthorizedAccessException("Invalid username/email or password");

        if (!_passwordHasher.Verify(loginRequest.Password, user.Password))
            throw new UnauthorizedAccessException("Invalid username/email or password");

        var authUserDto = _mapper.Map<AuthUserDto>(user);

        var accessToken = await _tokenService.GenerateAccessToken(authUserDto);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.UserId,
            ExpiryTime = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            IsRevoked = false,
            IsUsed = false
        };

        _unitOfWork.GetRepository<IRefreshTokenRepository>().Add(refreshTokenEntity);
        await _unitOfWork.SaveChangesAsync();

        return new AuthenticationResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AuthUserDto = authUserDto
        };
    }

    public async Task<AuthenticationResponse> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        var (newAccessToken, newRefreshToken, authUserDto) = await _tokenService.RefreshTokenAsync(accessToken, refreshToken);

        return new AuthenticationResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            AuthUserDto = authUserDto
        };
    }
    public async Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword) || !IsValidPassword(newPassword))
        {
            throw new ArgumentException("New password is not valid. Must be at least 8 characters, include uppercase, lowercase, number, and special character.");
        }

        var user = await _unitOfWork.GetRepository<IUserRepository>().GetByIdAsync(userId);
        if (user == null || user.IsDeleted)
        {
            throw new KeyNotFoundException("User not found.");
        }

        if (!_passwordHasher.Verify(currentPassword, user.Password))
        {
            throw new UnauthorizedAccessException("Current password is incorrect.");
        }

        user.Password = _passwordHasher.Hash(newPassword);
        user.LastUpdatedTime = DateTimeOffset.UtcNow;
        user.LastUpdatedBy = userId.ToString();

        _unitOfWork.GetRepository<IUserRepository>().Update(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task LogoutAsync(string refreshToken)
    {
        await _tokenService.RevokeRefreshTokenAsync(refreshToken);
    }

    public async Task<string> RegisterAsync(RegisterRequest registerRequest)
    {
        return await RegisterUserInternalAsync(registerRequest, "Customer");
    }

    public async Task<string> RegisterForManagerAsync(RegisterRequest registerRequest)
    {
        return await RegisterUserInternalAsync(registerRequest, "Manager");
    }

    public async Task<string> RegisterForStaffAsync(RegisterRequest registerRequest)
    {
        return await RegisterUserInternalAsync(registerRequest, "Staff");
    }

    private async Task<string> RegisterUserInternalAsync(RegisterRequest registerRequest, string roleName)
    {
        if (await _userService.CheckUserNameExistsAsync(registerRequest.UserName))
            throw new UnauthorizedAccessException("Username đã được sử dụng");

        if (await _userService.CheckEmailExistsAsync(registerRequest.EmailAddress))
            throw new UnauthorizedAccessException("Email đã được sử dụng");

        if (string.IsNullOrWhiteSpace(registerRequest.Password) || !IsValidPassword(registerRequest.Password))
        {
            throw new Exception("Password không hợp lệ. Phải có ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt.");
        }

        var userForCreationDto = _mapper.Map<UserForCreationDto>(registerRequest);

        userForCreationDto.Password = _passwordHasher.Hash(registerRequest.Password);
        userForCreationDto.Status = "Active";

        UserDto? createdUser = null;

        try
        {
            createdUser = await _userService.CreateAsync(userForCreationDto);
            await AssignRoleToUser(createdUser.UserId.ToString(), roleName);
            return createdUser.UserId.ToString();
        }
        catch (Exception ex)
        {
            if (createdUser != null)
                await _userService.DeleteAsync(createdUser.UserId);

            throw new ApplicationException("Đăng ký thất bại", ex);
        }
    }

    public async Task AssignRoleToUser(string userId, string roleName)
    {
        var userGuid = Guid.Parse(userId);
        var user = await _unitOfWork.GetRepository<IUserRepository>().GetByIdAsync(userGuid);

        if (user == null || user.IsDeleted)
            throw new KeyNotFoundException($"User với ID {userId} không tồn tại");

        RoleDto roleDto;
        try
        {
            roleDto = await _roleService.GetByNameAsync(roleName);
        }
        catch (KeyNotFoundException)
        {
            roleDto = await _roleService.CreateAsync(new RoleForCreationDto { RoleName = roleName });
        }

        user.RoleId = roleDto.RoleId;
        user.LastUpdatedTime = DateTimeOffset.UtcNow;
        user.LastUpdatedBy = "System";

        _unitOfWork.GetRepository<IUserRepository>().Update(user);
        await _unitOfWork.SaveChangesAsync();
    }

    private bool IsValidPassword(string password)
    {
        return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+={}[\]|\\:;'\<>,.?/~`])[A-Za-z\d!@#$%^&*()_+={}[\]|\\:;'\<>,.?/~`]{8,}$");
    }
}
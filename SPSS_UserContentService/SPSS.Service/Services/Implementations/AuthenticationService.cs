using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SPSS.BusinessObject.Dto.Authentication;
using SPSS.BusinessObject.Dto.Role;
using SPSS.BusinessObject.Dto.User;
using SPSS.BusinessObject.Dto.VerifyOtp;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Constants;
using SPSS.Shared.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SPSS.Service.Services.Implementations;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IConfiguration _configuration;
    private readonly EmailSender _emailSender;
    public AuthenticationService(IConfiguration configuration, EmailSender emailSender, IRoleService roleService, IUserService userService, IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper, IPasswordHasher passwordHasher)
    {
        _configuration = configuration;
        _emailSender = emailSender;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _mapper = mapper;
        _userService = userService;
        _roleService = roleService;
        _passwordHasher = passwordHasher;
    }

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
            throw new UnauthorizedAccessException(ExceptionMessageConstants.Authentication.InvalidCredentials);

        if (!_passwordHasher.Verify(loginRequest.Password, user.Password))
            throw new UnauthorizedAccessException(ExceptionMessageConstants.Authentication.InvalidCredentials);

        var authUserDto = _mapper.Map<AuthUserDto>(user);

        var accessToken = _tokenService.GenerateAccessToken(authUserDto);
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
            throw new ArgumentException(ExceptionMessageConstants.Authentication.InvalidPasswordFormat);
        }

        var user = await _unitOfWork.GetRepository<IUserRepository>().GetByIdAsync(userId);
        if (user == null || user.IsDeleted)
        {
            throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.User.NotFound, userId));
        }

        if (!_passwordHasher.Verify(currentPassword, user.Password))
        {
            throw new UnauthorizedAccessException(ExceptionMessageConstants.Authentication.CurrentPasswordIncorrect);
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

    public async Task<AuthUserDto> RegisterAsync(RegisterRequest registerRequest)
    {
        return await RegisterUserInternalAsync(registerRequest, "Customer");
    }

    public async Task<AuthUserDto> RegisterPrivilegedUserAsync(PrivilegedRegisterRequest registerRequest)
    {
        return await RegisterUserInternalAsync(registerRequest, registerRequest.RoleName);
    }

    private async Task<AuthUserDto> RegisterUserInternalAsync(RegisterRequest registerRequest, string roleName)
    {
        if (await _userService.CheckUserNameExistsAsync(registerRequest.UserName))
            throw new InvalidOperationException(string.Format(ExceptionMessageConstants.Authentication.UsernameTaken, registerRequest.UserName));

        if (await _userService.CheckEmailExistsAsync(registerRequest.EmailAddress))
            throw new InvalidOperationException(string.Format(ExceptionMessageConstants.Authentication.EmailTaken, registerRequest.EmailAddress));

        if (string.IsNullOrWhiteSpace(registerRequest.Password) || !IsValidPassword(registerRequest.Password))
        {
            throw new ArgumentException(ExceptionMessageConstants.Authentication.InvalidPasswordFormat);
        }

        var userForCreationDto = _mapper.Map<UserForCreationDto>(registerRequest);

        userForCreationDto.Password = _passwordHasher.Hash(registerRequest.Password);
        userForCreationDto.Status = "Active";

        UserDto? createdUser = null;

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            createdUser = await _userService.CreateAsync(userForCreationDto);
            if (createdUser == null)
            {
                throw new ApplicationException(ExceptionMessageConstants.Authentication.RegistrationFailed);
            }

            await AssignRoleToUser(createdUser.UserId.ToString(), roleName);

            await _unitOfWork.CommitTransactionAsync();
            await SendVerificationOtpAsync(createdUser.UserId, createdUser.EmailAddress);

            var mapItem = _mapper.Map<AuthUserDto>(createdUser);

            return mapItem;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw new ApplicationException(string.Format(ExceptionMessageConstants.Authentication.RegistrationFailed, ex.Message), ex);
        }
    }

    public async Task AssignRoleToUser(string userId, string roleName)
    {
        var userGuid = Guid.Parse(userId);
        var user = await _unitOfWork.GetRepository<IUserRepository>().GetByIdAsync(userGuid);

        if (user == null || user.IsDeleted)
            throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.User.NotFound, userId));

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
        return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>/?]).{8,}$");
    }

    public async Task SendVerificationOtpAsync(Guid userId, string email)
    {
        var otpLength = int.Parse(_configuration["Otp:Length"] ?? "6");
        var expiryMinutes = int.Parse(_configuration["Otp:ExpiryMinutes"] ?? "10");
        var key = _configuration["Otp:Key"] ?? throw new InvalidOperationException("Otp key not configured");

        var code = OtpHelper.GenerateNumericOtp(otpLength);
        var salt = OtpHelper.CreateSalt();
        var hashed = OtpHelper.HashOtp(code, key, salt);

        var verificationDto = new EmailVerificationForCreationDto
        {
            UserId = userId,
            Email = email,
            CodeHash = hashed,
            Salt = salt,
            ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(expiryMinutes)
        };

        var verification = _mapper.Map<EmailVerification>(verificationDto);

        await _unitOfWork.GetRepository<IEmailVerificationRepository>().RevokeAllByUserAsync(userId);

        _unitOfWork.GetRepository<IEmailVerificationRepository>().Add(verification);
        await _unitOfWork.SaveChangesAsync();

        var subject = "Mã xác thực tài khoản của bạn";
        var body = $"<p>Xin chào,</p><p>Mã xác thực (OTP) của bạn là: <strong>{code}</strong></p>" +
                   $"<p>Mã có hiệu lực trong {expiryMinutes} phút.</p>";

        await _emailSender.SendEmailAsync(email, subject, body);
    }

    public async Task VerifyAccountByOtpAsync(string email, string code)
    {
        var repo = _unitOfWork.GetRepository<IEmailVerificationRepository>();
        var verification = await repo.GetLatestByEmailAsync(email);
        if (verification == null)
            throw new KeyNotFoundException("Verification record not found.");

        if (verification.IsUsed || verification.IsRevoked)
            throw new InvalidOperationException("This verification code is no longer valid.");

        if (verification.ExpiresAt < DateTimeOffset.UtcNow)
        {
            verification.IsRevoked = true;
            repo.Update(verification);
            await _unitOfWork.SaveChangesAsync();
            throw new InvalidOperationException("Verification code expired.");
        }

        var key = _configuration["Otp:Key"] ?? throw new InvalidOperationException("Otp key not configured");
        var hashed = OtpHelper.HashOtp(code, key, verification.Salt);

        if (hashed != verification.CodeHash)
        {
            verification.Attempts++;
            if (verification.Attempts >= int.Parse(_configuration["Otp:MaxAttempts"] ?? "5"))
            {
                verification.IsRevoked = true;
            }
            repo.Update(verification);
            await _unitOfWork.SaveChangesAsync();
            throw new UnauthorizedAccessException("Invalid verification code.");
        }

        verification.IsUsed = true;
        repo.Update(verification);

        var user = await _unitOfWork.GetRepository<IUserRepository>().GetByIdAsync(verification.UserId);
        if (user == null) throw new KeyNotFoundException("User not found.");
        user.Status = "Active";
        _unitOfWork.GetRepository<IUserRepository>().Update(user);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ResendVerificationOtpAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        var user = await _unitOfWork.GetRepository<IUserRepository>().Entities
                     .FirstOrDefaultAsync(u => u.EmailAddress == email && !u.IsDeleted);

        if (user == null)
            throw new KeyNotFoundException("User not found.");

        if (string.Equals(user.Status, "Active", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Account already activated.");

        var emailVerRepo = _unitOfWork.GetRepository<IEmailVerificationRepository>();
        var last = await emailVerRepo.GetLatestByEmailAsync(email);

        var resendCooldownSec = int.Parse(_configuration["Otp:ResendCooldownSeconds"] ?? "60");
        if (last != null && (DateTimeOffset.UtcNow - last.CreatedAt).TotalSeconds < resendCooldownSec)
        {
            throw new InvalidOperationException($"Please wait before requesting another code. Try again in {resendCooldownSec} seconds.");
        }

        await emailVerRepo.RevokeAllByUserAsync(user.UserId);
        await _unitOfWork.SaveChangesAsync();

        var otpLength = int.Parse(_configuration["Otp:Length"] ?? "6");
        var expiryMinutes = int.Parse(_configuration["Otp:ExpiryMinutes"] ?? "10");
        var key = _configuration["Otp:Key"] ?? throw new InvalidOperationException("Otp key not configured");

        var code = OtpHelper.GenerateNumericOtp(otpLength);
        var salt = OtpHelper.CreateSalt();
        var hashed = OtpHelper.HashOtp(code, key, salt);

        var verificationDto = new EmailVerificationForCreationDto
        {
            UserId = user.UserId,
            Email = email,
            CodeHash = hashed,
            Salt = salt,
            ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(expiryMinutes)
        };
        var verification = _mapper.Map<EmailVerification>(verificationDto);

        emailVerRepo.Add(verification);
        await _unitOfWork.SaveChangesAsync();

        var subject = "Mã xác thực tài khoản của bạn";
        var body = $"<p>Xin chào {user.UserName},</p><p>Mã xác thực (OTP) của bạn là: <strong>{code}</strong></p>" +
                   $"<p>Mã có hiệu lực trong {expiryMinutes} phút.</p>";

        await _emailSender.SendEmailAsync(email, subject, body);
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SPSS.BusinessObject.Dto.Authentication;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Constants;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Services.Implementations;

public class TokenService : ITokenService
{
	private readonly IConfiguration _configuration;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly TimeSpan _accessTokenExpiration;
	private readonly TimeSpan _refreshTokenExpiration;

	public TokenService(IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper)
	{
		_configuration = configuration;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_accessTokenExpiration = TimeSpan.FromDays(double.Parse(_configuration["Jwt:AccessTokenExpirationDays"] ?? "30"));
		_refreshTokenExpiration = TimeSpan.FromDays(double.Parse(_configuration["Jwt:RefreshTokenExpirationDays"] ?? "7"));
	}

	public string GenerateAccessToken(AuthUserDto user)
	{
		var claims = new List<Claim>
		{
			new Claim("Id", user.UserId.ToString()),
			new Claim("UserName", user.UserName),
			new Claim("Email", user.EmailAddress),
			new Claim("AvatarUrl", user.AvatarUrl ?? string.Empty),
			new Claim("Role", user.Role ?? string.Empty)
		};

		var jwtKey = _configuration["Jwt:Key"];
		if (string.IsNullOrEmpty(jwtKey))
			throw new InvalidOperationException(ExceptionMessageConstants.Token.JwtKeyNotConfigured);

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		var expires = DateTime.UtcNow.Add(_accessTokenExpiration);

		var token = new JwtSecurityToken(
			issuer: _configuration["Jwt:Issuer"],
			audience: _configuration["Jwt:Audience"],
			claims: claims,
			expires: expires,
			signingCredentials: credentials
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	public string GenerateRefreshToken()
	{
		var randomNumber = new byte[64];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}

	public bool ValidateAccessToken(string token, out Guid userId)
	{
		userId = Guid.Empty;

		try
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtKey = _configuration["Jwt:Key"];
			if (string.IsNullOrEmpty(jwtKey))
				throw new InvalidOperationException(ExceptionMessageConstants.Token.JwtKeyNotConfigured);

			var key = Encoding.UTF8.GetBytes(jwtKey);
			tokenHandler.ValidateToken(token, new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidIssuer = _configuration["Jwt:Issuer"],
				ValidAudience = _configuration["Jwt:Audience"],
				ClockSkew = TimeSpan.Zero
			}, out SecurityToken validatedToken);

			var jwtToken = (JwtSecurityToken)validatedToken;
			var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;


			if (Guid.TryParse(userIdClaim, out userId))
			{
				return true;
			}

			return false;
		}
		catch
		{
			return false;
		}
	}

	public async Task<(string accessToken, string refreshToken, AuthUserDto authUserDto)> RefreshTokenAsync(string accessToken, string refreshToken)
	{
		var storedRefreshToken = await _unitOfWork.GetRepository<IRefreshTokenRepository>().GetByTokenAsync(refreshToken);

		if (storedRefreshToken == null || storedRefreshToken.IsUsed || storedRefreshToken.IsRevoked || storedRefreshToken.ExpiryTime < DateTimeOffset.UtcNow)
			throw new SecurityTokenException(ExceptionMessageConstants.Token.InvalidRefreshToken);

		storedRefreshToken.IsUsed = true;
		_unitOfWork.GetRepository<IRefreshTokenRepository>().Update(storedRefreshToken);

		var user = await _unitOfWork.GetRepository<IUserRepository>().Entities
			.Include(u => u.Role)
			.FirstOrDefaultAsync(u => u.UserId == storedRefreshToken.UserId);

		if (user == null || user.IsDeleted)
			throw new SecurityTokenException(ExceptionMessageConstants.Token.UserNotFound);

		var authUserDto = _mapper.Map<AuthUserDto>(user);

		var newAccessToken = GenerateAccessToken(authUserDto);
		var newRefreshToken = GenerateRefreshToken();

		var refreshTokenEntity = new RefreshToken
		{
			Token = newRefreshToken,
			UserId = user.UserId,
			ExpiryTime = DateTime.UtcNow.Add(_refreshTokenExpiration),
			Created = DateTime.UtcNow,
			IsRevoked = false,
			IsUsed = false
		};

		_unitOfWork.GetRepository<IRefreshTokenRepository>().Add(refreshTokenEntity);
		await _unitOfWork.SaveChangesAsync();

		return (newAccessToken, newRefreshToken, authUserDto);
	}

	public async Task RevokeRefreshTokenAsync(string refreshToken)
	{
		var storedRefreshToken = await _unitOfWork.GetRepository<IRefreshTokenRepository>().GetByTokenAsync(refreshToken);

		if (storedRefreshToken == null)
			throw new SecurityTokenException(ExceptionMessageConstants.Token.InvalidRefreshTokenSimple);

		storedRefreshToken.IsRevoked = true;
		_unitOfWork.GetRepository<IRefreshTokenRepository>().Update(storedRefreshToken);
		await _unitOfWork.SaveChangesAsync();
	}
}

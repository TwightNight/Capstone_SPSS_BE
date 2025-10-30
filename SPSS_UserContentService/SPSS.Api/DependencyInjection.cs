using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SPSS.BusinessObject.Context;
using SPSS.Repository.Repositories.Implementations;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Implementations;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Service.Implementations;
using SPSS.Service.Interfaces;
using SPSS.Service.Services.Implementations;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Base.Implementations;
using SPSS.Shared.Base.Interfaces;
using SPSS.Shared.Helpers;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace SPSS.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddHttpContextAccessor();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        var jwtKey = configuration["JwtSettings:Key"];
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new InvalidOperationException("JWT Key is not configured in appsettings.json under JwtSettings:Key");
        }

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ClockSkew = TimeSpan.Zero,
                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.Name
                };

                // Workaround: dùng JwtSecurityTokenHandler thay vì JsonWebTokenHandler
                options.SecurityTokenValidators.Clear();
                options.SecurityTokenValidators.Add(new JwtSecurityTokenHandler());

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                                          .GetRequiredService<ILoggerFactory>()
                                          .CreateLogger("JwtAuth");
                        logger.LogError(context.Exception, "Jwt authentication failed: {Message}", context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                                          .GetRequiredService<ILoggerFactory>()
                                          .CreateLogger("JwtAuth");
                        logger.LogInformation("Token validated. Claims: {Claims}",
                            string.Join(", ", context.Principal.Claims.Select(c => $"{c.Type}={c.Value}")));
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserDBContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IBlogRepository, BlogRepository>();
        services.AddScoped<IBlogSectionRepository, BlogSectionRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IChatHistoryRepository, ChatHistoryRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ISkinConditionRepository, SkinConditionRepository>();
        services.AddScoped<ISkinTypeRepository, SkinTypeRepository>();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //services.AddAutoMapper(typeof(UserService).Assembly);
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<EmailSender>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IChatHistoryService, ChatHistoryService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ISkinConditionService, SkinConditionService>();
        services.AddScoped<ISkinTypeService, SkinTypeService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        services.AddScoped<IEmailVerificationRepository, EmailVerificationRepository>();


        return services;
    }
}
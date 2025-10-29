// Trong dự án SPSS.Api
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
using System.Reflection;
using System.Text;

namespace SPSS.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Đăng ký Controllers
        services.AddControllers();

        // 2. Đăng ký HttpContextAccessor (rất phổ biến)
        // Giúp bạn có thể inject IHttpContextAccessor vào các Service
        services.AddHttpContextAccessor();

        // 3. Cấu hình CORS (Chính sách Cross-Origin)
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());
            // Hoặc một chính sách chặt chẽ hơn cho production
        });

        // 4. Cấu hình Swagger/OpenAPI (để test API)
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            // Đây là ví dụ cấu hình Swagger để hỗ trợ JWT
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

        // 5. Cấu hình Authentication (JWT)
        // Đọc cấu hình JWT từ appsettings.json
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
                };
            });

        //// 6. Cấu hình Authorization (Policies)
        //services.AddAuthorization(options =>
        //{
        //    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
        //    // ... Thêm các policy khác
        //});

        return services;
    }
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserDBContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnectionString")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IBlogRepository, BlogRepository>();
        services.AddScoped<IBlogSectionRepository, BlogSectionRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IChatHistoryRepository, ChatHistoryRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISkinConditionRepository, SkinConditionRepository>();
        services.AddScoped<ISkinTypeRepository, SkinTypeRepository>();

        return services;
    }
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

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
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserService, UserService>();


        return services;
    }
}
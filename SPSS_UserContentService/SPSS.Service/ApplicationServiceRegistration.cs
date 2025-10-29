using Microsoft.Extensions.DependencyInjection;
using SPSS.Service.Implementations;
using SPSS.Service.Interfaces;
using SPSS.Service.Services.Implementations;
using SPSS.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service;

public static class ApplicationServiceRegistration
{
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

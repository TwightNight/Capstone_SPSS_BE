using Microsoft.Extensions.DependencyInjection;
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


        return services;
    }
}

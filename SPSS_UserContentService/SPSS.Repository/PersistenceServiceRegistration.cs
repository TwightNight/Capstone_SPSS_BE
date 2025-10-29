using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SPSS.BusinessObject.Context;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Shared.Base.Implementations;
using SPSS.Shared.Base.Interfaces;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.Repositories.Implementations;

namespace SPSS.Repository;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserDBContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnectionString")));

        services.AddScoped<IUnitOfWork, UnitOfWork.Implementations.UnitOfWork>();
     
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
}

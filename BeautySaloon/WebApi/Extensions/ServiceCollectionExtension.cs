using BeautySaloon.DAL;
using BeautySaloon.DAL.Providers;
using BeautySaloon.DAL.Repositories.Abstract;
using BeautySaloon.DAL.Uow;
using BeautySaloon.WebApi.Providers;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.WebApi.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDatabaseLayer<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
    {
        services.AddDbContext<BeautySaloonDbContext>(options);

        services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
        services.AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>));
        services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        return services;
    }
}

using BeautySaloon.DAL.Interceptors;
using BeautySaloon.DAL.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BeautySaloon.DAL;
public class BeautySaloonDbContext : DbContext
{
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly ILoggerFactory _loggerFactory;

    public BeautySaloonDbContext(
        DbContextOptions options,
        ILoggerFactory loggerFactory,
        ICurrentUserProvider currentUserProvider)
        : base(options)
    {
        _loggerFactory = loggerFactory;
        _currentUserProvider = currentUserProvider;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(
            new AuditInterceptor(_currentUserProvider),
            new SoftDeleteInterceptor());

        optionsBuilder.UseLoggerFactory(_loggerFactory);

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        base.OnModelCreating(modelBuilder);
    }
}

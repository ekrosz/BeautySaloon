using BeautySaloon.DAL.Interceptors;
using BeautySaloon.DAL.Providers;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.DAL;
public class BeautySaloonDbContext : DbContext
{
    private readonly ICurrentUserProvider _currentUserProvider;

    public BeautySaloonDbContext(DbContextOptions options, ICurrentUserProvider currentUserProvider)
        : base(options)
    {
        _currentUserProvider = currentUserProvider;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(
            new AuditInterceptor(_currentUserProvider),
            new SoftDeleteInterceptor());

        base.OnConfiguring(optionsBuilder);
    }
}

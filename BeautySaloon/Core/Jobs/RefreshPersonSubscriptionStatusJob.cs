using BeautySaloon.Core.Settings;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Repositories.Abstract;
using BeautySaloon.DAL.Uow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BeautySaloon.Core.Jobs;

public class RefreshPersonSubscriptionStatusJob : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RefreshPersonSubscriptionStatusJob(IServiceScopeFactory serviceScopeFactory)
        => _serviceScopeFactory = serviceScopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var scope = _serviceScopeFactory.CreateScope();

            var personSubscriptionWriteRepository = scope.ServiceProvider.GetRequiredService<IWriteRepository<PersonSubscription>>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var settings = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<BLayerSettings>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<RefreshPersonSubscriptionStatusJob>>();

            logger.LogInformation("Запущен мониторинг просроченных услуг абонементов.");

            try
            {
                var personSubscriptions = await personSubscriptionWriteRepository.FindAsync(
                    x => x.Status == PersonSubscriptionCosmeticServiceStatus.Paid
                        && x.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.LifeTimeInDays.HasValue
                        && x.Order.UpdatedOn.AddDays(x.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.LifeTimeInDays.Value) < DateTime.UtcNow,
                    stoppingToken);

                foreach (var personSubscription in personSubscriptions)
                {
                    personSubscription.Overdue();

                    await unitOfWork.SaveChangesAsync(stoppingToken);

                    logger.LogInformation("Услуга абонемента клиента {0} была переведена в статус \"Просрочено\" в связи с истечением срока актуальности абонемента: {1}.", personSubscription.Id, personSubscription);
                }

                logger.LogInformation("Мониторинг просроченных услуг абонементов окончен успешно.");

                await Task.Delay(TimeSpan.FromDays(settings.Value.ExecuteJobPeriodInDays), stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }
    }
}

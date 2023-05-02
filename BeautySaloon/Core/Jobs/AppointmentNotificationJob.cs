using BeautySaloon.Core.IntegrationServices.MailKit.Contracts;
using BeautySaloon.Core.IntegrationServices.MailKit.Dto;
using BeautySaloon.Core.Settings;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Repositories.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BeautySaloon.Core.Jobs;

public class AppointmentNotificationJob : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AppointmentNotificationJob(IServiceScopeFactory serviceScopeFactory)
        => _serviceScopeFactory = serviceScopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var scope = _serviceScopeFactory.CreateScope();

            var appointmentReadRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<Appointment>>();
            var mailKitService = scope.ServiceProvider.GetRequiredService<IMailKitService>();
            var settings = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<BLayerSettings>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppointmentNotificationJob>>();

            logger.LogInformation("Запущены работа с нотификациями для клиентов о предстоящих записях.");

            try
            {
                var utcNow = DateTime.UtcNow;

                var appointments = (await appointmentReadRepository.FindAsync(
                        x => x.AppointmentDate.Date == utcNow.Date
                        && x.PersonSubscriptions.Any()
                        && x.PersonSubscriptions.All(y => y.Status == PersonSubscriptionCosmeticServiceStatus.InProgress),
                        stoppingToken))
                    .Where(x => DateTime.SpecifyKind(x.AppointmentDate, DateTimeKind.Local).ToUniversalTime().AddHours(-2).ToString("HH:mm").Equals(utcNow.ToString("HH:mm"), StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                foreach (var appointment in appointments)
                {
                    if (string.IsNullOrEmpty(appointment.Person.Email))
                    {
                        logger.LogInformation("Клиент {0} {1} не имеет электронной почты для нотификаций.", appointment.Person.PhoneNumber, appointment.Person.Name.ConcatedName);
                    }

                    await mailKitService.SendEmailAsync(new SendEmailRequestDto
                    {
                        ReceiverName = appointment.Person.Name.ConcatedName,
                        ReceiverEmail = appointment.Person.Email!,
                        Subject = $"Запись в Beauty Studio на {appointment.AppointmentDate:g}",
                        Body = $"Здравствуйте, {appointment.Person.Name.ConcatedName}!\n" +
                               $"Напоминаем, что вы записаны на оказание услуг в студии красоты Beauty Studio на сегодня в {appointment.AppointmentDate:t}. Просьба прийти за 10 минут до назначенного времени."
                    }, stoppingToken);

                    logger.LogInformation("Успешно отправлено уведомление клиенту {0} {1} о предстоящей записи", appointment.Person.PhoneNumber, appointment.Person.Name.ConcatedName);
                }

                logger.LogInformation("Нотификации для клиентов о предстоящих записях успешно отправлены.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            finally
            {
                await Task.Delay(TimeSpan.FromMinutes(settings.Value.JobSettings.AppointmentNotificationJobInMinutes), stoppingToken);
            }
        }
    }
}

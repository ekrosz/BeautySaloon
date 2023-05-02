using BeautySaloon.Core.IntegrationServices.MailKit.Dto;

namespace BeautySaloon.Core.IntegrationServices.MailKit.Contracts;

public interface IMailKitService
{
    Task SendEmailAsync(SendEmailRequestDto request, CancellationToken cancellationToken = default);
}

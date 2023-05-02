using BeautySaloon.Core.IntegrationServices.MailKit.Contracts;
using BeautySaloon.Core.IntegrationServices.MailKit.Dto;
using BeautySaloon.Core.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BeautySaloon.Core.IntegrationServices.MailKit;

public class MailKitService : IMailKitService
{
    private const string PlainType = "plain";

    private const string HtmlType = "html";

    private const string MultipartType = "mixed";

    private const string MediaType = "application";

    private const string MediaSubType = "pdf";

    private readonly MailKitSettings _mailKitSettings;

    public MailKitService(IOptionsSnapshot<BLayerSettings> settings)
    {
        _mailKitSettings = settings.Value.MailKitSettings;
    }

    public async Task SendEmailAsync(SendEmailRequestDto request, CancellationToken cancellationToken = default)
    {
        var mailMessage = new MimeMessage();

        mailMessage.From.Add(new MailboxAddress(_mailKitSettings.SenderName, _mailKitSettings.SenderEmail));
        mailMessage.To.Add(new MailboxAddress(request.ReceiverName, request.ReceiverEmail));

        var body = new Multipart(MultipartType);

        body.Add(new TextPart(request.IsHtmlContent ? HtmlType : PlainType)
        {
            Text = request.Body
        });

        var fileStreams = request.Files
            .Select(x => (FileName: x.FileName, Stream: new MemoryStream(x.Data)))
            .ToList();

        foreach (var fileStream in fileStreams)
        {
            body.Add(new MimePart(MediaType, MediaSubType)
            {
                Content = new MimeContent(fileStream.Stream),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = fileStream.FileName
            });
        }

        mailMessage.Subject = request.Subject;
        mailMessage.Body = body;

        using var smtpClient = new SmtpClient();

        await smtpClient.ConnectAsync(_mailKitSettings.Host, _mailKitSettings.Port, false, cancellationToken);
        await smtpClient.AuthenticateAsync(_mailKitSettings.Username, _mailKitSettings.Password, cancellationToken);

        await smtpClient.SendAsync(mailMessage, cancellationToken);
        await smtpClient.DisconnectAsync(true, cancellationToken);

        fileStreams.ForEach(x => x.Stream.Dispose());
    }
}

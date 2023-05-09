using BeautySaloon.Core.Utils.Contracts;
using BeautySaloon.Core.Utils.Dto;
using BeautySaloon.DAL.Entities.Enums;
using TemplateEngine.Docx;

namespace BeautySaloon.Core.Utils;

public class OrderReportDocumentGenerator : DocumentGenerator<OrderReportRequestDto>
{
    private const string DocumentName = "OrderReport";

    private const string TemplatePostfix = "Template";

    private const string DocxExtension = ".docx";

    protected override Task<DocumentGeneratorResponseDto> GenerateContentAsync(string fileName, OrderReportRequestDto data)
    {
        var templateFileName = DocumentName + TemplatePostfix + DocxExtension;
        var tempFileName = fileName + DocxExtension;

        File.Delete(tempFileName);
        File.Copy(templateFileName, tempFileName);

        var table = new TableContent(nameof(data.Items));

        foreach (var item in data.Items)
        {
            table.AddRow(
                new FieldContent(nameof(item.PersonFullName), item.PersonFullName),
                new FieldContent(nameof(item.EmployeeFullName), item.EmployeeFullName),
                new FieldContent(nameof(item.SubscriptionNames), item.SubscriptionNames),
                new FieldContent(nameof(item.PaymentStatus), ToDisplayName(item.PaymentStatus)),
                new FieldContent(nameof(item.Cost), $"{item.Cost} руб."),
                new FieldContent(nameof(item.CreatedOn), item.CreatedOn.ToString("dd.MM.yyyy")));
        }

        var content = new Content(
            new FieldContent(nameof(data.StartCreatedOn), data.StartCreatedOn.ToString("dd.MM.yyyy")),
            new FieldContent(nameof(data.EndCreatedOn), data.EndCreatedOn.ToString("dd.MM.yyyy")),
            new FieldContent(nameof(data.TotalCost), data.TotalCost.ToString()),
            table);

        return Task.FromResult(new DocumentGeneratorResponseDto { FileName = tempFileName, Content = content });
    }

    private string ToDisplayName(PaymentStatus status) => status switch
    {
        PaymentStatus.NotPaid => "Не оплачено",
        PaymentStatus.Paid => "Оплачено",
        PaymentStatus.Cancelled => "Отменено",
        _ => string.Empty
    };
}

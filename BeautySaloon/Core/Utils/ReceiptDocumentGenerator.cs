using BeautySaloon.Core.Utils.Contracts;
using BeautySaloon.Core.Utils.Dto;
using TemplateEngine.Docx;

namespace BeautySaloon.Core.Utils;

public class ReceiptDocumentGenerator : DocumentGenerator<ReceiptRequestDto>
{
    private const string DocumentName = "Receipt";

    private const string TemplatePostfix = "Template";

    private const string DocxExtension = ".docx";

    protected override Task<DocumentGeneratorResponseDto> GenerateContentAsync(ReceiptRequestDto data)
    {
        var templateFileName = DocumentName + TemplatePostfix + DocxExtension;
        var tempFileName = DocumentName + DocxExtension;

        File.Delete(tempFileName);
        File.Copy(templateFileName, tempFileName);

        var table = new TableContent(nameof(data.Items));

        foreach (var item in data.Items)
        {
            table.AddRow(
                new FieldContent(nameof(item.Name), item.Name),
                new FieldContent(nameof(item.Count), item.Count.ToString()),
                new FieldContent(nameof(item.Price), item.Price.ToString()),
                new FieldContent(nameof(item.TotalPrice), item.TotalPrice.ToString()));
        }

        var content = new Content(
            new FieldContent(nameof(data.Number), data.Number.ToString()),
            new FieldContent(nameof(data.PersonFullName), data.PersonFullName),
            new FieldContent(nameof(data.PersonPhoneNumber), data.PersonPhoneNumber),
            new FieldContent(nameof(data.PaidOn), data.PaidOn.ToString("dd.MM.yyyy")),
            new FieldContent(nameof(data.Cost), data.Cost.ToString()),
            table);

        return Task.FromResult(new DocumentGeneratorResponseDto { FileName = tempFileName, Content = content });
    }
}

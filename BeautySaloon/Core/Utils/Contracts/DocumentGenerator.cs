using BeautySaloon.Api.Dto.Common;
using GrapeCity.Documents.Word;
using GrapeCity.Documents.Word.Layout;
using System.IO.Compression;
using TemplateEngine.Docx;

namespace BeautySaloon.Core.Utils.Contracts;

public abstract class DocumentGenerator<T> : IDocumentGenerator<T> where T : class
{
    private const string PdfExtension = ".pdf";

    public async Task<FileResponseDto> GenerateDocumentAsync(string fileName, T data)
    {
        var result = await GenerateContentAsync(fileName, data);

        var pdfFileName = fileName + PdfExtension;

        using var outputDocument = new TemplateProcessor(result.FileName).SetRemoveContentControls(true);

        outputDocument.FillContent(result.Content);
        outputDocument.SaveChanges();

        var wordDoc = new GcWordDocument();

        wordDoc.Load(result.FileName);

        using var layout = new GcWordLayout(wordDoc);

        var pdfSettings = new PdfOutputSettings
        {
            CompressionLevel = CompressionLevel.Fastest,
            ConformanceLevel = GrapeCity.Documents.Pdf.PdfAConformanceLevel.PdfA1a
        };

        layout.SaveAsPdf(pdfFileName, null, pdfSettings);

        File.Delete(result.FileName);

        using var fileStream = new FileStream(pdfFileName, FileMode.OpenOrCreate);
        using var memoryStream = new MemoryStream();

        await fileStream.CopyToAsync(memoryStream);

        return new FileResponseDto { FileName = pdfFileName, Data = memoryStream.ToArray() };
    }

    protected abstract Task<DocumentGeneratorResponseDto> GenerateContentAsync(string fileName, T data);
}

public record DocumentGeneratorResponseDto
{
    public string FileName { get; init; } = string.Empty;

    public Content Content { get; init; } = null!;
}

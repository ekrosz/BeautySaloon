using BeautySaloon.Api.Dto.Requests.Order;
using BeautySaloon.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers;

[ApiController]
[Route("api/files")]
public class FileController : ControllerBase
{
    private readonly IOrderHttpClient _orderHttpClient;

    public FileController(IOrderHttpClient orderHttpClient)
    {
        _orderHttpClient = orderHttpClient;
    }

    [HttpGet("receipt")]
    public async Task<IActionResult> GetReceiptAsync(string accessToken, Guid id, CancellationToken cancellationToken = default)
    {
        var receipt = await _orderHttpClient.GetReceiptAsync(accessToken, id, cancellationToken);

        return File(receipt.Data, contentType: "application/pdf", receipt.FileName);
    }

    [HttpGet("report")]
    public async Task<IActionResult> GetReportAsync(string accessToken, DateTime startCreatedOn, DateTime endCreatedOn, CancellationToken cancellationToken = default)
    {
        var receipt = await _orderHttpClient.GetReportAsync(accessToken, new GetOrderReportRequestDto { StartCreatedOn = startCreatedOn, EndCreatedOn = endCreatedOn }, cancellationToken);

        return File(receipt.Data, contentType: "application/pdf", receipt.FileName);
    }
}

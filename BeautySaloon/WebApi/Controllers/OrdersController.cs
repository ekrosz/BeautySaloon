using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.Order;
using BeautySaloon.Api.Dto.Responses.Order;
using BeautySaloon.Common;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.WebApi.Controllers;

[Route("api/orders")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    private readonly IValidator<CreateOrderRequestDto> _createOrderRequestValidator;
    private readonly IValidator<UpdateOrderRequestDto> _updateOrderRequestValidator;
    private readonly IValidator<PayOrderRequestDto> _payOrderRequestValidator;
    private readonly IValidator<CancelOrderRequestDto> _cancelOrderRequestValidator;
    private readonly IValidator<GetOrderListRequestDto> _getOrderListRequestValidator;
    private readonly IValidator<GetOrderReportRequestDto> _getOrderReportRequestValidator;
    private readonly IValidator<GetOrderAnalyticRequestDto> _getOrderAnalyticRequestValidator;
    private readonly IValidator<ByIdRequestDto> _byIdRequestValidator;

    public OrdersController(
        IOrderService orderService,
        IValidator<CreateOrderRequestDto> createOrderRequestValidator,
        IValidator<UpdateOrderRequestDto> updateOrderRequestValidator,
        IValidator<PayOrderRequestDto> payOrderRequestValidator,
        IValidator<CancelOrderRequestDto> cancelOrderRequestValidator,
        IValidator<GetOrderListRequestDto> getOrderListRequestValidator,
        IValidator<GetOrderReportRequestDto> getOrderReportRequestValidator,
        IValidator<GetOrderAnalyticRequestDto> getOrderAnalyticRequestValidator,
        IValidator<ByIdRequestDto> byIdRequestValidator)
    {
        _orderService = orderService;
        _createOrderRequestValidator = createOrderRequestValidator;
        _updateOrderRequestValidator = updateOrderRequestValidator;
        _payOrderRequestValidator = payOrderRequestValidator;
        _cancelOrderRequestValidator = cancelOrderRequestValidator;
        _getOrderListRequestValidator = getOrderListRequestValidator;
        _getOrderReportRequestValidator = getOrderReportRequestValidator;
        _getOrderAnalyticRequestValidator = getOrderAnalyticRequestValidator;
        _byIdRequestValidator = byIdRequestValidator;
    }

    [HttpPost]
    [Authorize(Roles = Constants.Roles.AdminAndEmployee)]
    public async Task CreateAsync([FromBody] CreateOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        await _createOrderRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        await _orderService.CreateOrderAsync(request, cancellationToken);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Constants.Roles.AdminAndEmployee)]
    public async Task UpdateAsync([FromRoute] Guid id, [FromBody] UpdateOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        await _updateOrderRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var requestById = new ByIdWithDataRequestDto<UpdateOrderRequestDto>(id, request);

        await _orderService.UpdateOrderAsync(requestById, cancellationToken);
    }

    [HttpPatch("{id}/pay")]
    [Authorize(Roles = Constants.Roles.AdminAndEmployee)]
    public async Task<PayOrderResponseDto> PayAsync([FromRoute] Guid id, [FromBody] PayOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        await _payOrderRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var requestById = new ByIdWithDataRequestDto<PayOrderRequestDto>(id, request);

        return await _orderService.PayOrderAsync(requestById, cancellationToken);
    }

    [HttpPatch("{id}/cancel")]
    [Authorize(Roles = Constants.Roles.AdminAndEmployee)]
    public async Task CancelAsync([FromRoute] Guid id, [FromBody] CancelOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        await _cancelOrderRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var requestById = new ByIdWithDataRequestDto<CancelOrderRequestDto>(id, request);

        await _orderService.CancelOrderAsync(requestById, cancellationToken);
    }

    [HttpGet]
    [Authorize(Roles = Constants.Roles.AdminAndEmployee)]
    public async Task<PageResponseDto<GetOrderResponseDto>> GetListAsync([FromQuery] GetOrderListRequestDto request, CancellationToken cancellationToken = default)
    {
        await _getOrderListRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        return await _orderService.GetOrderListAsync(request, cancellationToken);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = Constants.Roles.AdminAndEmployee)]
    public async Task<GetOrderResponseDto> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById, cancellationToken);

        return await _orderService.GetOrderAsync(requestById, cancellationToken);
    }

    [HttpGet("{id}/payment-status")]
    [Authorize(Roles = Constants.Roles.AdminAndEmployee)]
    public async Task<CheckAndUpdateOrderPaymentStatusResponseDto> CheckAndUpdatePaymentStatusAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById, cancellationToken);

        return await _orderService.CheckAndUpdateOrderPaymentStatusAsync(requestById, cancellationToken);
    }

    [HttpGet("{id}/receipt")]
    [Authorize(Roles = Constants.Roles.AdminAndEmployee)]
    public async Task<FileResponseDto> GetReceiptAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById, cancellationToken);

        return await _orderService.GetOrderReceiptAsync(requestById, cancellationToken);
    }

    [HttpGet("report")]
    [Authorize(Roles = Constants.Roles.AdminAndEmployee)]
    public async Task<FileResponseDto> GetReportAsync([FromQuery] GetOrderReportRequestDto request, CancellationToken cancellationToken = default)
    {
        await _getOrderReportRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        return await _orderService.GetOrderReportAsync(request, cancellationToken);
    }

    [HttpGet("analytic")]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task<GetOrderAnalyticResponseDto> GetAnalyticAsync([FromQuery] GetOrderAnalyticRequestDto request, CancellationToken cancellationToken = default)
    {
        await _getOrderAnalyticRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        return await _orderService.GetOrderAnalyticAsync(request, cancellationToken);
    }
}

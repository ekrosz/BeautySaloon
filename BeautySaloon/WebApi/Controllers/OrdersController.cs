using BeautySaloon.Common;
using BeautySaloon.Core.Dto.Common;
using BeautySaloon.Core.Dto.Requests.Order;
using BeautySaloon.Core.Dto.Responses.Order;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.WebApi.Controllers;

[Authorize(Roles = Constants.Roles.AdminAndEmployee)]
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
    private readonly IValidator<ByIdRequestDto> _byIdRequestValidator;

    public OrdersController(
        IOrderService orderService,
        IValidator<CreateOrderRequestDto> createOrderRequestValidator,
        IValidator<UpdateOrderRequestDto> updateOrderRequestValidator,
        IValidator<PayOrderRequestDto> payOrderRequestValidator,
        IValidator<CancelOrderRequestDto> cancelOrderRequestValidator,
        IValidator<GetOrderListRequestDto> getOrderListRequestValidator,
        IValidator<ByIdRequestDto> byIdRequestValidator)
    {
        _orderService = orderService;
        _createOrderRequestValidator = createOrderRequestValidator;
        _updateOrderRequestValidator = updateOrderRequestValidator;
        _payOrderRequestValidator = payOrderRequestValidator;
        _cancelOrderRequestValidator = cancelOrderRequestValidator;
        _getOrderListRequestValidator = getOrderListRequestValidator;
        _byIdRequestValidator = byIdRequestValidator;
    }

    [HttpPost]
    public async Task CreateAsync([FromBody] CreateOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        await _createOrderRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        await _orderService.CreateOrderAsync(request, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync([FromRoute] Guid id, [FromBody] UpdateOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        await _updateOrderRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var requestById = new ByIdWithDataRequestDto<UpdateOrderRequestDto>(id, request);

        await _orderService.UpdateOrderAsync(requestById, cancellationToken);
    }

    [HttpPatch("{id}/pay")]
    public async Task PayAsync([FromRoute] Guid id, [FromBody] PayOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        await _payOrderRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var requestById = new ByIdWithDataRequestDto<PayOrderRequestDto>(id, request);

        await _orderService.PayOrderAsync(requestById, cancellationToken);
    }

    [HttpPatch("{id}/cancel")]
    public async Task CancelAsync([FromRoute] Guid id, [FromBody] CancelOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        await _cancelOrderRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var requestById = new ByIdWithDataRequestDto<CancelOrderRequestDto>(id, request);

        await _orderService.CancelOrderAsync(requestById, cancellationToken);
    }

    [HttpGet]
    public async Task<PageResponseDto<GetOrderResponseDto>> GetListAsync([FromQuery] GetOrderListRequestDto request, CancellationToken cancellationToken = default)
    {
        await _getOrderListRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        return await _orderService.GetOrderListAsync(request, cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<GetOrderResponseDto> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById, cancellationToken);

        return await _orderService.GetOrderAsync(requestById, cancellationToken);
    }
}

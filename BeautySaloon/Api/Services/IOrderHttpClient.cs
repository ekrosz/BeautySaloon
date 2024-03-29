﻿using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.Order;
using BeautySaloon.Api.Dto.Responses.Order;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using Refit;

namespace BeautySaloon.Api.Services;

[Headers("Content-Type: application/json")]
public interface IOrderHttpClient
{
    [Post("/api/orders")]
    Task CreateAsync([Header("Authorization")] string accessToken, [Body] CreateOrderRequestDto request, CancellationToken cancellationToken = default);

    [Put("/api/orders/{id}")]
    Task UpdateAsync([Header("Authorization")] string accessToken, Guid id, [Body] UpdateOrderRequestDto request, CancellationToken cancellationToken = default);

    [Patch("/api/orders/{id}/pay")]
    Task<PayOrderResponseDto> PayAsync([Header("Authorization")] string accessToken, Guid id, [Body] PayOrderRequestDto request, CancellationToken cancellationToken = default);

    [Patch("/api/orders/{id}/cancel")]
    Task CancelAsync([Header("Authorization")] string accessToken, Guid id, [Body] CancelOrderRequestDto request, CancellationToken cancellationToken = default);

    [Get("/api/orders")]
    Task<PageResponseDto<GetOrderResponseDto>> GetListAsync([Header("Authorization")] string accessToken, [Query] GetOrderListRequestDto request, CancellationToken cancellationToken = default);

    [Get("/api/orders/{id}")]
    Task<GetOrderResponseDto> GetAsync([Header("Authorization")] string accessToken, Guid id, CancellationToken cancellationToken = default);

    [Get("/api/orders/{id}/payment-status")]
    Task<CheckAndUpdateOrderPaymentStatusResponseDto> CheckAndUpdatePaymentStatusAsync([Header("Authorization")] string accessToken, Guid id, CancellationToken cancellationToken = default);

    [Get("/api/orders/{id}/receipt")]
    Task<FileResponseDto> GetReceiptAsync([Header("Authorization")] string accessToken, Guid id, CancellationToken cancellationToken = default);

    [Get("/api/orders/report")]
    Task<FileResponseDto> GetReportAsync([Header("Authorization")] string accessToken, [Query] GetOrderReportRequestDto request, CancellationToken cancellationToken = default);

    [Get("/api/orders/analytic")]
    Task<GetOrderAnalyticResponseDto> GetAnalyticAsync([Header("Authorization")] string accessToken, [Query] GetOrderAnalyticRequestDto request, CancellationToken cancellationToken = default);
}

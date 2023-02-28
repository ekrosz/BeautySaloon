using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;

namespace BeautySaloon.Core.Dto.Requests.Order;

public class GetOrderRequestDto
{
    public string? SearchString { get; set; } = string.Empty;

    public PaymentStatus? PaymentStatus { get; set; }

    public DateTime? StartCreatedOn { get; set; }

    public DateTime? EndCreatedOn { get; set; }

    public PageRequestDto Page { get; set; } = new();
}

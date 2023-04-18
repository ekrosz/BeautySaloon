using Newtonsoft.Json;

namespace BeautySaloon.Core.Api.SmartPay.Dto.CreateInvoice;

public record CreateInvoiceRequestDto
{
    [JsonProperty("user_id")]
    public UserRequestDto UserId { get; init; } = default!;

    [JsonProperty("ptype")]
    public int Type { get; init; }

    [JsonProperty("invoice")]
    public InvoiceRequestDto Invoice { get; init; } = default!;

    public record UserRequestDto
    {
        [JsonProperty("partner_client_id")]
        public Guid PartnerClientId { get; init; }
    }

    public record InvoiceRequestDto
    {
        [JsonProperty("purchaser")]
        public PurchaserRequestDto Purchaser { get; init; } = default!;

        [JsonProperty("invoice_params")]
        public IReadOnlyCollection<InvoiceParameterRequestDto> InvoiceParameters { get; init; } = Array.Empty<InvoiceParameterRequestDto>();

        [JsonProperty("order")]
        public OrderRequestDto Order { get; init; } = default!;

        public record PurchaserRequestDto
        {
            [JsonProperty("email")]
            public string? Email { get; init; }

            [JsonProperty("phone")]
            public string Phone { get; init; } = default!;

            [JsonProperty("contact")]
            public string Contact { get; init; } = default!;
        }

        public record InvoiceParameterRequestDto
        {
            [JsonProperty("key")]
            public string Key { get; init; } = default!;

            [JsonProperty("value")]
            public string Value { get; init; } = default!;
        }

        public record OrderRequestDto
        {
            [JsonProperty("order_id")]
            public Guid OrderId { get; init; }

            [JsonProperty("order_number")]
            public string OrderNumber { get; init; } = default!;

            [JsonProperty("order_date")]
            public string OrderDate { get; init; } = default!;

            [JsonProperty("service_id")]
            public string ServiceId { get; init; } = default!;

            [JsonProperty("amount")]
            public int Amount { get; init; }

            [JsonProperty("currency")]
            public string Currency { get; init; } = default!;

            [JsonProperty("purpose")]
            public string? Purpose { get; init; }

            [JsonProperty("description")]
            public string? Description { get; init; }

            [JsonProperty("language")]
            public string Language { get; init; } = default!;

            [JsonProperty("expiration_date")]
            public DateTime ExpirationDate { get; init; } = default!;

            [JsonProperty("tax_system")]
            public int TaxSystem { get; init; }

            [JsonProperty("order_bundle")]
            public IReadOnlyCollection<OrderBundleRequestDto> OrderBundles { get; init; } = Array.Empty<OrderBundleRequestDto>();

            public record OrderBundleRequestDto
            {
                [JsonProperty("position_id")]
                public int PositionId { get; init; }

                [JsonProperty("name")]
                public string Name { get; init; } = default!;

                [JsonProperty("item_params")]
                public IReadOnlyCollection<ItemParameterRequestDto> ItemParameters { get; init; } = Array.Empty<ItemParameterRequestDto>();

                [JsonProperty("quantity")]
                public QuantityRequestDto Quantity { get; init; } = default!;

                [JsonProperty("item_amount")]
                public int ItemAmount { get; init; }

                [JsonProperty("currency")]
                public string Currency { get; init; } = default!;

                [JsonProperty("item_code")]
                public Guid ItemCode { get; init; } = default!;

                [JsonProperty("item_price")]
                public int ItemPrice { get; init; }

                [JsonProperty("tax_type")]
                public int TaxType { get; init; }

                [JsonProperty("tax_sum")]
                public int TaxSum { get; init; }

                public record ItemParameterRequestDto
                {
                    [JsonProperty("key")]
                    public string Key { get; init; } = default!;

                    [JsonProperty("value")]
                    public string Value { get; init; } = default!;
                }

                public record QuantityRequestDto
                {
                    [JsonProperty("value")]
                    public int Value { get; init; } = default!;

                    [JsonProperty("measure")]
                    public string Measure { get; init; } = default!;
                }
            }
        }
    }
}

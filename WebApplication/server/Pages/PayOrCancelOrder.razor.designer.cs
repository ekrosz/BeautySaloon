using BeautySaloon.Api.Services;
using BeautySaloon.DAL.Entities.Enums;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using WebApplication.Wrappers;
using BeautySaloon.Api.Dto.Requests.Order;

namespace WebApplication.Pages;

public partial class PayOrCancelOrderComponent : ComponentBase
{
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

    public void Reload()
    {
        InvokeAsync(StateHasChanged);
    }

    [Inject]
    protected IJSRuntime JSRuntime { get; set; }

    [Inject]
    protected DialogService DialogService { get; set; }

    [Inject]
    protected TooltipService TooltipService { get; set; }

    [Inject]
    protected ContextMenuService ContextMenuService { get; set; }

    [Inject]
    protected NotificationService NotificationService { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    protected IOrderHttpClient OrderHttpClient { get; set; }

    [Inject]
    protected IHttpClientWrapper HttpClientWrapper { get; set; }

    [Parameter]
    public Guid Id { get; set; }

    [Parameter]
    public bool IsPayOperation { get; set; }

    protected string PageTitle => IsPayOperation
        ? "Оплата заказа"
        : "Отмена заказа";

    private PayOrCancelRequest _payOrCancelOperation;

    protected PayOrCancelRequest PayOrCancelOperation
    {
        get
        {
            return _payOrCancelOperation;
        }
        set
        {
            if (!object.Equals(_payOrCancelOperation, value))
            {
                _payOrCancelOperation = value;
                Reload();
            }
        }
    }

    protected static IDictionary<PaymentMethod, string> PaymentMethods = Enum.GetValues<PaymentMethod>()
            .Where(x => x != PaymentMethod.None)
            .ToDictionary(k => k, v => v switch
            {
                PaymentMethod.Cash => "Наличные",
                PaymentMethod.Card => "Банковская карта",
                _ => string.Empty
            });

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Load();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    protected Task Load()
    {
        PayOrCancelOperation = new PayOrCancelRequest();

        return Task.CompletedTask;
    }

    protected async Task Form0Submit()
    {
        if (IsPayOperation)
        {
            var paymentResponse = await HttpClientWrapper.SendAsync((accessToken) => OrderHttpClient.PayAsync(accessToken, Id, new PayOrderRequestDto { Comment = PayOrCancelOperation.Comment, PaymentMethod = PayOrCancelOperation.PaymentMethod }, CancellationToken.None));

            if (paymentResponse == default)
            {
                DialogService.Close(false);
                return;
            }

            if (paymentResponse.QrCode == null)
            {
                NotificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Заказ успешно оплачен"
                });

                DialogService.Close(true);
                return;
            }

            var dialogResult = await DialogService.OpenAsync<CheckOrderPaymentStatus>("Оплата заказа", new Dictionary<string, object>() { { "Id", Id }, { "QrCodeImage", paymentResponse.QrCode } });

            DialogService.Close((dialogResult as bool?).GetValueOrDefault());

            return;
        }

        var isSuccess = await HttpClientWrapper.SendAsync((accessToken) => OrderHttpClient.CancelAsync(accessToken, Id, new CancelOrderRequestDto { Comment = PayOrCancelOperation.Comment }, CancellationToken.None));

        if (isSuccess)
        {
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Success,
                Summary = "Заказ успешно отменен"
            });
        }

        DialogService.Close(isSuccess);
    }

    protected async Task Button2Click(MouseEventArgs args)
    {
        DialogService.Close(false);
    }

    public record PayOrCancelRequest
    {
        public string? Comment { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
    }
}

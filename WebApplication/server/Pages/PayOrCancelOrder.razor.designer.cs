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

    private string? _comment;

    protected string? Comment
    {
        get
        {
            return _comment;
        }
        set
        {
            if (!object.Equals(_comment, value))
            {
                _comment = value;
                Reload();
            }
        }
    }

    private PaymentMethod _paymentMethod;

    protected PaymentMethod PaymentMethod
    {
        get
        {
            return _paymentMethod;
        }
        set
        {
            if (!object.Equals(_paymentMethod, value))
            {
                _paymentMethod = value;
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

    protected async Task Form0Submit(string args)
    {
        var isSuccess = IsPayOperation
            ? await HttpClientWrapper.SendAsync((accessToken) => OrderHttpClient.PayAsync(accessToken, Id, new PayOrderRequestDto { Comment = Comment, PaymentMethod = PaymentMethod }, CancellationToken.None))
            : await HttpClientWrapper.SendAsync((accessToken) => OrderHttpClient.CancelAsync(accessToken, Id, new CancelOrderRequestDto { Comment = Comment }, CancellationToken.None));

        DialogService.Close(isSuccess);
    }

    protected async Task Button2Click(MouseEventArgs args)
    {
        DialogService.Close(false);
    }
}

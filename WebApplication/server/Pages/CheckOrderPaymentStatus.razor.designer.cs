using BeautySaloon.Api.Services;
using BeautySaloon.DAL.Entities.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using WebApplication.Wrappers;

namespace WebApplication.Pages;

public partial class CheckOrderPaymentStatusComponent : ComponentBase
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
    public byte[] QrCodeImage { get; set; }

    protected async Task Form0Submit()
    {
        var response = await HttpClientWrapper.SendAsync((accessToken) => OrderHttpClient.CheckAndUpdatePaymentStatusAsync(accessToken, Id, CancellationToken.None));

        if (response == default)
        {
            return;
        }

        if (response.PaymentStatus == PaymentStatus.NotPaid)
        {
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Info,
                Summary = "Заказ еще не оплачен"
            });

            return;
        }

        if (response.PaymentStatus == PaymentStatus.Paid)
        {
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Success,
                Summary = "Заказ успешно оплачен"
            });

            DialogService.Close(true);
            return;
        }

        if (response.PaymentStatus == PaymentStatus.Cancelled)
        {
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Warning,
                Summary = "Заказ был отменен"
            });

            DialogService.Close(true);
            return;
        }
    }
}

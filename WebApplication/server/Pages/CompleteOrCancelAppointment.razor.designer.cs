using BeautySaloon.Api.Dto.Requests.Appointment;
using BeautySaloon.Api.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using WebApplication.Wrappers;

namespace WebApplication.Pages;

public partial class CompleteOrCancelAppointmentComponent : ComponentBase
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
    protected IAppointmentHttpClient AppointmentHttpClient { get; set; }

    [Inject]
    protected IHttpClientWrapper HttpClientWrapper { get; set; }

    [Parameter]
    public Guid Id { get; set; }

    [Parameter]
    public bool IsCompleteOperation { get; set; }

    protected string PageTitle => IsCompleteOperation
        ? "Выполнение записи"
        : "Отмена записи";

    private CompleteOrCancelRequest _completeOrCancelOperation;

    protected CompleteOrCancelRequest CompleteOrCancelOperation
    {
        get
        {
            return _completeOrCancelOperation;
        }
        set
        {
            if (!object.Equals(_completeOrCancelOperation, value))
            {
                _completeOrCancelOperation = value;
                Reload();
            }
        }
    }

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
        CompleteOrCancelOperation = new CompleteOrCancelRequest();

        return Task.CompletedTask;
    }

    protected async Task Form0Submit()
    {
        var isSuccess = IsCompleteOperation
            ? await HttpClientWrapper.SendAsync((accessToken) => AppointmentHttpClient.CompleteAsync(accessToken, Id, new CompleteOrCancelAppointmentRequestDto { Comment = CompleteOrCancelOperation.Comment }, CancellationToken.None))
            : await HttpClientWrapper.SendAsync((accessToken) => AppointmentHttpClient.CancelAsync(accessToken, Id, new CompleteOrCancelAppointmentRequestDto { Comment = CompleteOrCancelOperation.Comment }, CancellationToken.None));

        DialogService.Close(isSuccess);
    }

    protected async Task Button2Click(MouseEventArgs args)
    {
        DialogService.Close(false);
    }

    public record CompleteOrCancelRequest
    {
        public string? Comment { get; set; }
    }
}

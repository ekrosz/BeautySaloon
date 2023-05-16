using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using BeautySaloon.Api.Services;
using WebApplication.Wrappers;
using BeautySaloon.Api.Dto.Responses.Appointment;
using BeautySaloon.Api.Dto.Requests.Appointment;
using Radzen.Blazor;
using BeautySaloon.DAL.Entities.Enums;

namespace WebApplication.Pages
{
    public partial class AppointmentsComponent : ComponentBase
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
        protected IAppointmentHttpClient AppointmentHttpClient { get; set; }

        [Inject]
        protected IHttpClientWrapper HttpClientWrapper { get; set; }

        protected RadzenScheduler<GetAppointmentListItemResponseDto> scheduler;

        private string _search;

        protected string Search
        {
            get
            {
                return _search;
            }
            set
            {
                if (!object.Equals(_search, value))
                {
                    _search = value;
                    Reload();
                }
            }
        }

        private IReadOnlyCollection<GetAppointmentListItemResponseDto> _getAppointmentsResult;

        protected IReadOnlyCollection<GetAppointmentListItemResponseDto> GetAppointmentsResult
        {
            get
            {
                return _getAppointmentsResult;
            }
            set
            {
                if (!object.Equals(_getAppointmentsResult, value))
                {
                    _getAppointmentsResult = value;
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

        protected async Task Load()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Search = string.Empty;
            }

            var appointments = await HttpClientWrapper.SendAsync((accessToken) => AppointmentHttpClient.GetListAsync(accessToken, new GetAppointmentListRequestDto { SearchString = Search }));

            if (appointments == default)
            {
                return;
            }

            GetAppointmentsResult = appointments.Items;
        }

        protected async Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddAppointment>("Создание записи", null);

            if ((dialogResult as bool?).GetValueOrDefault())
            {
                await Load();
                await InvokeAsync(() => { StateHasChanged(); });

                NotificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Запись успешно сохранена"
                });
            }
        }

        protected void OnSlotRender(SchedulerSlotRenderEventArgs args)
        {
            if (args.View.Text.Equals("Month"))
            {
                if (args.Start.Date == DateTime.Today)
                {
                    args.Attributes["style"] = "background: rgba(255,220,40,.2);";
                }

                if (args.Start.Date < DateTime.Today)
                {
                    args.Attributes["style"] = "background: rgba(130,130,130,.2);";
                }
            }

            if ((args.View.Text.Equals("Week") || args.View.Text.Equals("Day")) && args.Start.Hour > 8 && args.Start.Hour < 19)
            {
                if (args.Start < DateTime.Now)
                {
                    args.Attributes["style"] = "background: rgba(130,130,130,.2);";
                    return;
                }

                args.Attributes["style"] = "background: rgba(255,220,40,.2);";
            }
        }

        protected void OnAppointmentRender(SchedulerAppointmentRenderEventArgs<GetAppointmentListItemResponseDto> args)
        {
            if (args.Data.Status == AppointmentStatus.NotImplemented && (args.Data.AppointmentDate - DateTime.Now).TotalHours <= 2)
            {
                args.Attributes["style"] = "background: rgba(120,0,0,.8);";

                return;
            }

            if (args.Data.Status == AppointmentStatus.Completed)
            {
                args.Attributes["style"] = "background: rgba(0,120,0,.7);";

                return;
            }

            args.Attributes["style"] = "background: rgba(0,113,135,.8);";
        }

        protected async Task OnSlotSelect(SchedulerSlotSelectEventArgs args)
        {
            if (args.Start < DateTime.Now)
            {
                return;
            }

            var dialogResult = await DialogService.OpenAsync<AddAppointment>("Создание записи", new Dictionary<string, object> { { "AppointmentDate", args.Start } });

            if ((dialogResult as bool?).GetValueOrDefault())
            {
                await Load();
                await InvokeAsync(() => { StateHasChanged(); });

                NotificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Запись успешно сохранена"
                });
            }
        }

        protected async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<GetAppointmentListItemResponseDto> args)
        {
            var dialogResult = await DialogService.OpenAsync<EditAppointment>("Редактирование записи", new Dictionary<string, object> { { "Id", args.Data.Id } });

            if ((dialogResult as bool?).GetValueOrDefault())
            {
                await Load();
                await scheduler.Reload();
                await InvokeAsync(() => { StateHasChanged(); });

                NotificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Запись успешно сохранена"
                });
            }
        }
    }
}

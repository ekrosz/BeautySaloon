using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using BeautySaloon.Api.Services;
using WebApplication.Wrappers;
using BeautySaloon.Api.Dto.Responses.Appointment;
using BeautySaloon.Api.Dto.Requests.Appointment;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
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

        protected RadzenDataGrid<GetAppointmentListItemResponseDto> grid0;

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

        private int _pageSize = 10;

        protected int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (!object.Equals(_pageSize, value))
                {
                    _pageSize = value;
                    Reload();
                }
            }
        }

        private int _pageNumber = 1;

        protected int PageNumber
        {
            get
            {
                return _pageNumber;
            }
            set
            {
                if (!object.Equals(_pageNumber, value))
                {
                    _pageNumber = value;
                    Reload();
                }
            }
        }

        private DateTime? _endCreatedOn = DateTime.Now.AddDays(7);

        protected DateTime? EndCreatedOn
        {
            get
            {
                return _endCreatedOn;
            }
            set
            {
                if (!object.Equals(_endCreatedOn, value))
                {
                    _endCreatedOn = value;
                    Reload();
                }
            }
        }

        private DateTime? _startCreatedOn = DateTime.Now;

        protected DateTime? StartCreatedOn
        {
            get
            {
                return _startCreatedOn;
            }
            set
            {
                if (!object.Equals(_startCreatedOn, value))
                {
                    _startCreatedOn = value;
                    Reload();
                }
            }
        }

        protected int TotalCount { get; set; }

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

            var appointments = await HttpClientWrapper.SendAsync((accessToken) => AppointmentHttpClient.GetListAsync(accessToken, new GetAppointmentListRequestDto
            {
                SearchString = Search,
                StartAppointmentDate = StartCreatedOn,
                EndAppointmentDate = EndCreatedOn,
                Page = new PageRequestDto(PageNumber, PageSize)
            }));

            if (appointments == default)
            {
                return;
            }

            GetAppointmentsResult = appointments.Items;
            TotalCount = appointments.TotalCount;
        }

        protected async Task LoadDataAsync(LoadDataArgs args)
        {
            PageSize = args.Top ?? PageSize;
            PageNumber = args.Skip.HasValue
            ? (args.Skip.Value / PageSize) + 1
            : PageNumber;

            await Load();
        }

        protected async Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddAppointment>("Создание записи", null);

            if ((dialogResult as bool?).GetValueOrDefault())
            {
                await Load();
                await grid0.Reload();
                await InvokeAsync(() => { StateHasChanged(); });

                NotificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Запись успешно сохранена"
                });
            }
        }

        protected async Task Grid0RowSelect(GetAppointmentListItemResponseDto args)
        {
            if (args.Status == AppointmentStatus.Cancelled)
            {
                return;
            }

            var dialogResult = await DialogService.OpenAsync<EditAppointment>("Редактирование записи", new Dictionary<string, object>() { { "Id", args.Id } });

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

        protected async Task GridCompleteButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<CompleteOrCancelAppointment>("Выполнение записи", new Dictionary<string, object> { { "Id", data.Id }, { "IsCompleteOperation", true } });

            if ((dialogResult as bool?).GetValueOrDefault())
            {
                await Load();
                await grid0.Reload();

                NotificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Заказ успешно оплачен"
                });
            }
        }

        protected async Task GridCancelButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<CompleteOrCancelAppointment>("Отмена записи", new Dictionary<string, object> { { "Id", data.Id }, { "IsCompleteOperation", false } });

            if ((dialogResult as bool?).GetValueOrDefault())
            {
                await Load();
                await grid0.Reload();

                NotificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Заказ успешно отменен"
                });
            }
        }

        protected void ShowCompleteButtonTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, "Выполнено", options);

        protected void ShowCancelButtonTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, "Отменить", options);
    }
}

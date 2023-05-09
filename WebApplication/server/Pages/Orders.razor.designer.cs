using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using BeautySaloon.Api.Services;
using WebApplication.Wrappers;
using BeautySaloon.Api.Dto.Responses.Order;
using BeautySaloon.Api.Dto.Requests.Order;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using BeautySaloon.DAL.Entities.Enums;

namespace WebApplication.Pages
{
    public partial class OrdersComponent : ComponentBase
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
        protected IUserHttpClient UserHttpClient { get; set; }

        [Inject]
        protected IHttpClientWrapper HttpClientWrapper { get; set; }

        protected RadzenDataGrid<GetOrderResponseDto> grid0;

        private const int StartAnalyticYear = 2018;

        protected bool IsTrendVisible { get; set; } = false;

        protected bool IsMedianVisible { get; set; } = false;

        protected bool IsMeanVisible { get; set; } = false;

        protected bool IsModeVisible { get; set; } = false;

        protected bool IsCountChart { get; set; } = false;

        private bool _isAnalyticBlockVisible = false;

        protected bool IsAnalyticBlockVisible
        {
            get
            {
                return _isAnalyticBlockVisible;
            }
            set
            {
                if (!object.Equals(_isAnalyticBlockVisible, value))
                {
                    _isAnalyticBlockVisible = value;
                    Reload();
                }
            }
        }

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

        private IReadOnlyCollection<GetOrderResponseDto> _getOrdersResult;

        protected IReadOnlyCollection<GetOrderResponseDto> GetOrdersResult
        {
            get
            {
                return _getOrdersResult;
            }
            set
            {
                if (!object.Equals(_getOrdersResult, value))
                {
                    _getOrdersResult = value;
                    Reload();
                }
            }
        }

        private IReadOnlyCollection<GetOrderAnalyticResponseDto.GetOrderAnalyticItemResponseDto> _currentAnalytic;

        protected IReadOnlyCollection<GetOrderAnalyticResponseDto.GetOrderAnalyticItemResponseDto> CurrentAnalytic
        {
            get
            {
                return _currentAnalytic;
            }
            set
            {
                if (!object.Equals(_currentAnalytic, value))
                {
                    _currentAnalytic = value;
                    Reload();
                }
            }
        }

        private IReadOnlyCollection<GetOrderAnalyticResponseDto.GetOrderAnalyticItemResponseDto> _forecastAnalytic;

        protected IReadOnlyCollection<GetOrderAnalyticResponseDto.GetOrderAnalyticItemResponseDto> ForecastAnalytic
        {
            get
            {
                return _forecastAnalytic;
            }
            set
            {
                if (!object.Equals(_forecastAnalytic, value))
                {
                    _forecastAnalytic = value;
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

        private DateTime? _endCreatedOn = DateTime.Now;

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

        private DateTime? _startCreatedOn = DateTime.Now.AddDays(-7);

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

        private decimal _totalValueAnalytic;

        protected decimal TotalValueAnalytic
        {
            get
            {
                return _totalValueAnalytic;
            }
            set
            {
                if (!object.Equals(_totalValueAnalytic, value))
                {
                    _totalValueAnalytic = value;
                    Reload();
                }
            }
        }

        private int _year = DateTime.Now.Year;

        protected int Year
        {
            get
            {
                return _year;
            }
            set
            {
                if (!object.Equals(_year, value))
                {
                    _year = value;
                    Reload();
                }
            }
        }

        protected int TotalCount { get; set; }

        protected static int[] Years = Enumerable.Range(StartAnalyticYear, DateTime.Now.Year - StartAnalyticYear + 1).ToArray();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Load();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        protected async Task LoadDataAsync(LoadDataArgs args)
        {
            PageSize = args.Top ?? PageSize;
            PageNumber = args.Skip.HasValue
            ? (args.Skip.Value / PageSize) + 1
            : PageNumber;

            await Load();
        }

        protected async Task Load()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Search = string.Empty;
            }

            var orders = await HttpClientWrapper.SendAsync((accessToken) => OrderHttpClient.GetListAsync(accessToken, new GetOrderListRequestDto
            {
                SearchString = Search,
                StartCreatedOn = StartCreatedOn?.ToUniversalTime(),
                EndCreatedOn = EndCreatedOn?.ToUniversalTime(),
                Page = new PageRequestDto(PageNumber, PageSize)
            }, CancellationToken.None));

            if (orders == default)
            {
                return;
            }

            GetOrdersResult = orders.Items;
            TotalCount = orders.TotalCount;

            var user = await HttpClientWrapper.SendAsync((accessToken) => UserHttpClient.GetAsync(accessToken, CancellationToken.None));

            if (user == default)
            {
                return;
            }

            IsAnalyticBlockVisible = user.Role == Role.Admin;

            await LoadAnalytic();
        }

        protected async Task LoadAnalytic()
        {
            if (!IsAnalyticBlockVisible)
            {
                return;
            }

            var analytic = await HttpClientWrapper.SendAsync((accessToken) => OrderHttpClient.GetAnalyticAsync(accessToken, new GetOrderAnalyticRequestDto { Year = Year }, CancellationToken.None));

            if (analytic == default)
            {
                return;
            }

            CurrentAnalytic = analytic.Items;
            ForecastAnalytic = analytic.ForecastItems;
            TotalValueAnalytic = analytic.TotalRevenues;
        }

        protected async Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddOrder>("Создание заказа", null);

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

        protected async Task Grid0RowSelect(GetOrderResponseDto args)
        {
            var dialogResult = await DialogService.OpenAsync<EditOrder>("Редактирование заказа", new Dictionary<string, object>() { {"Id", args.Id} });

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

        protected async Task GridPayButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<PayOrCancelOrder>("Оплата заказа", new Dictionary<string, object> { { "Id", data.Id }, { "IsPayOperation", true } });

            if ((dialogResult as bool?).GetValueOrDefault())
            {
                await Load();
                await grid0.Reload();
            }
        }

        protected async Task GridCancelButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<PayOrCancelOrder>("Отмена заказа", new Dictionary<string, object> { { "Id", data.Id }, { "IsPayOperation", false } });

            if ((dialogResult as bool?).GetValueOrDefault())
            {
                await Load();
                await grid0.Reload();
            }
        }

        protected Task GridDownloadReceiptButtonClick(MouseEventArgs args, dynamic data)
            => HttpClientWrapper.SendAsync((accessToken) => Task.Run(() => JSRuntime.InvokeAsync<object>("open", Path.Combine(NavigationManager.BaseUri, $"/api/files/receipt?accessToken={accessToken}&id={data.Id}"), "_blank")));

        protected Task GridDownloadReportButtonClick(MouseEventArgs args)
            => HttpClientWrapper.SendAsync((accessToken) => Task.Run(() => JSRuntime.InvokeAsync<object>("open", Path.Combine(NavigationManager.BaseUri, $"/api/files/report?accessToken={accessToken}&startCreatedOn={StartCreatedOn?.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")}&endCreatedOn={EndCreatedOn?.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")}"), "_blank")));

        protected void ShowPayButtonTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, "Оплатить", options);

        protected void ShowCancelButtonTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, "Отменить", options);

        protected void ShowDownloadButtonTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, "Скачать", options);

        protected string FormatAsMonth(object value) => value != null ? Convert.ToDateTime(value).ToString("MMM") : string.Empty;
    }
}

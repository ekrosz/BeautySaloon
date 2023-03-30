using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using BeautySaloon.Api.Services;
using BeautySaloon.Api.Dto.Responses.CosmeticService;
using BeautySaloon.Api.Dto.Requests.CosmeticService;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using WebApplication.Handlers;
using WebApplication.Wrappers;

namespace WebApplication.Pages
{
    public partial class CosmeticServicesComponent : ComponentBase
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
        protected ICosmeticServiceHttpClient CosmeticServiceHttpClient { get; set; }

        [Inject]
        protected IHttpClientWrapper HttpClientWrapper { get; set; }

        protected RadzenDataGrid<GetCosmeticServiceResponseDto> grid0;

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

        private IReadOnlyCollection<GetCosmeticServiceResponseDto> _getCosmeticServicesResult;

        protected IReadOnlyCollection<GetCosmeticServiceResponseDto> GetCosmeticServicesResult
        {
            get
            {
                return _getCosmeticServicesResult;
            }
            set
            {
                if (!object.Equals(_getCosmeticServicesResult, value))
                {
                    _getCosmeticServicesResult = value;
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

        protected int TotalCount { get; set; }

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

            var cosmeticServices = await HttpClientWrapper.SendAsync((accessToken)
                => CosmeticServiceHttpClient.GetListAsync(accessToken, new GetCosmeticServiceListRequestDto { Page = new PageRequestDto(PageNumber, PageSize), SearchString = Search }, CancellationToken.None));

            if (cosmeticServices == default)
            {
                return;
            }

            GetCosmeticServicesResult = cosmeticServices.Items;
            TotalCount = cosmeticServices.TotalCount;
        }

        protected async Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddCosmeticService>("Создание услуги", null);

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

        protected async Task Grid0RowSelect(GetCosmeticServiceResponseDto args)
        {
            var dialogResult = await DialogService.OpenAsync<EditCosmeticService>("Редактирование услуги", new Dictionary<string, object>() { {"Id", args.Id} });

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

        protected async Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
            {
                var isSuccess = await HttpClientWrapper.SendAsync((accessToken) => CosmeticServiceHttpClient.DeleteAsync(accessToken, data.Id, CancellationToken.None));

                if (!isSuccess)
                {
                    return;
                }

                await Load();
                await grid0.Reload();

                NotificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Запись успешно удалена"
                });
            }
        }
    }
}

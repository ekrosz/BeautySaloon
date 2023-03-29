using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using BeautySaloon.Api.Dto.Responses.Subscription;
using BeautySaloon.Api.Services;
using BeautySaloon.Api.Dto.Requests.Subscription;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using WebApplication.Handlers;
using Azure.Core;
using WebApplication.Wrappers;

namespace WebApplication.Pages
{
    public partial class SubscriptionsComponent : ComponentBase
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
        protected ISubscriptionHttpClient SubscriptionHttpClient { get; set; }

        [Inject]
        protected IHttpClientWrapper HttpClientWrapper { get; set; }

        protected RadzenDataGrid<GetSubscriptionListItemResponseDto> grid0;

        string _search;

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

        IReadOnlyCollection<GetSubscriptionListItemResponseDto> _getSubscriptionsResult;

        protected IReadOnlyCollection<GetSubscriptionListItemResponseDto> GetSubscriptionsResult
        {
            get
            {
                return _getSubscriptionsResult;
            }
            set
            {
                if (!object.Equals(_getSubscriptionsResult, value))
                {
                    _getSubscriptionsResult = value;
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

            var subscriptions = await HttpClientWrapper.SendAsync((accessToken)
                => SubscriptionHttpClient.GetListAsync(accessToken, new GetSubscriptionListRequestDto { Page = new PageRequestDto(PageNumber, PageSize), SearchString = Search }, CancellationToken.None));

            if (subscriptions == default)
            {
                return;
            }

            GetSubscriptionsResult = subscriptions.Items;
            TotalCount = subscriptions.TotalCount;
            
        }

        protected async Task Button0Click(MouseEventArgs args)
        {
            NavigationManager.NavigateTo($"/add-subscription");
        }

        protected async Task Grid0RowSelect(GetSubscriptionListItemResponseDto args)
        {
            NavigationManager.NavigateTo($"/edit-subscription/{args.Id}");
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
            {
                await HttpClientWrapper.SendAsync((accessToken) => SubscriptionHttpClient.DeleteAsync(accessToken, data.Id, CancellationToken.None));

                await grid0.Reload();
                await Load();

                NotificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Запись успешно удалена"
                });
            }
        }
    }
}

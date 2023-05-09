using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using BeautySaloon.Api.Dto.Responses.Invoice;
using BeautySaloon.Api.Services;
using BeautySaloon.Api.Dto.Requests.Invoice;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using WebApplication.Handlers;
using Azure.Core;
using WebApplication.Wrappers;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities;

namespace WebApplication.Pages
{
    public partial class InvoicesComponent : ComponentBase
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
        protected IInvoiceHttpClient InvoiceHttpClient { get; set; }

        [Inject]
        protected IHttpClientWrapper HttpClientWrapper { get; set; }

        protected RadzenDataGrid<GetInvoiceListItemResponseDto> grid0;

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

        private IReadOnlyCollection<GetInvoiceListItemResponseDto> _getInvoicesResult;

        protected IReadOnlyCollection<GetInvoiceListItemResponseDto> GetInvoicesResult
        {
            get
            {
                return _getInvoicesResult;
            }
            set
            {
                if (!object.Equals(_getInvoicesResult, value))
                {
                    _getInvoicesResult = value;
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

        private DateTime? _endCreatedOn = DateTime.Now.AddDays(1);

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

        private InvoiceType? _type;

        protected InvoiceType? Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (!object.Equals(_type, value))
                {
                    _type = value;
                    Reload();
                }
            }
        }

        protected int TotalCount { get; set; }

        protected static IDictionary<InvoiceType, string> InvoiceTypes = Enum.GetValues<InvoiceType>()
           .ToDictionary(k => k, v => v switch
           {
               InvoiceType.Extradition => "Расход",
               InvoiceType.Receipt => "Приход",
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

            var invoices = await HttpClientWrapper.SendAsync((accessToken) => InvoiceHttpClient.GetListAsync(accessToken, new GetInvoiceListRequestDto
            {
                SearchString = Search,
                InvoiceType = Type,
                StartCreatedOn = StartCreatedOn?.ToUniversalTime(),
                EndCreatedOn = EndCreatedOn?.ToUniversalTime(),
                Page = new PageRequestDto(PageNumber, PageSize)
            }, CancellationToken.None));

            if (invoices == default)
            {
                return;
            }

            GetInvoicesResult = invoices.Items;
            TotalCount = invoices.TotalCount;

        }

        protected async Task Button0Click(MouseEventArgs args)
        {
            NavigationManager.NavigateTo($"/add-invoice");
        }

        protected async Task Grid0RowSelect(GetInvoiceListItemResponseDto args)
        {
            NavigationManager.NavigateTo($"/edit-invoice/{args.Id}");
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            if (await DialogService.Confirm("Вы действительно хотите удалить запись?") == true)
            {
                var isSuccess = await HttpClientWrapper.SendAsync((accessToken) => InvoiceHttpClient.DeleteAsync(accessToken, data.Id, CancellationToken.None));

                if (!isSuccess)
                {
                    return;
                }

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

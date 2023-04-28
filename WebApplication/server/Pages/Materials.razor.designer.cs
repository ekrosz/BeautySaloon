using BeautySaloon.Api.Dto.Requests.Material;
using BeautySaloon.Api.Dto.Responses.Material;
using BeautySaloon.Api.Services;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using WebApplication.Wrappers;

namespace WebApplication.Pages
{
    public partial class MaterialsComponent : ComponentBase
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
        protected IMaterialHttpClient MaterialHttpClient { get; set; }

        [Inject]
        protected IHttpClientWrapper HttpClientWrapper { get; set; }

        protected RadzenDataGrid<GetMaterialResponseDto> grid0;

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

        private IReadOnlyCollection<GetMaterialResponseDto> _getMaterialsResult;

        protected IReadOnlyCollection<GetMaterialResponseDto> GetMaterialsResult
        {
            get
            {
                return _getMaterialsResult;
            }
            set
            {
                if (!object.Equals(_getMaterialsResult, value))
                {
                    _getMaterialsResult = value;
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

            var materials = await HttpClientWrapper.SendAsync((accessToken)
                => MaterialHttpClient.GetListAsync(accessToken, new GetMaterialListRequestDto { Page = new PageRequestDto(PageNumber, PageSize), SearchString = Search }, CancellationToken.None));

            if (materials == default)
            {
                return;
            }

            GetMaterialsResult = materials.Items;
            TotalCount = materials.TotalCount;
        }

        protected async Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddMaterial>("Создание материала", null);

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

        protected async Task Grid0RowSelect(GetMaterialResponseDto args)
        {
            var dialogResult = await DialogService.OpenAsync<EditMaterial>("Редактирование материала", new Dictionary<string, object>() { { "Id", args.Id } });

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
            if (await DialogService.Confirm("Вы действительно хотите удалить запись?") == true)
            {
                var isSuccess = await HttpClientWrapper.SendAsync((accessToken) => MaterialHttpClient.DeleteAsync(accessToken, data.Id, CancellationToken.None));

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

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using WebApplication.Models.LocalDb;
using Microsoft.EntityFrameworkCore;
using WebApplication.Services;
using BeautySaloon.Api.Services;
using BeautySaloon.Api.Dto.Responses.Person;
using BeautySaloon.Api.Dto.Requests.Person;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using WebApplication.Handlers;

namespace WebApplication.Pages
{
    public partial class PeopleComponent : ComponentBase
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
        protected NavigationManager UriHelper { get; set; }

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
        protected IPersonHttpClient PersonHttpClient { get; set; }

        protected RadzenDataGrid<GetPersonListItemResponseDto> grid0;

        private string _search;

        protected string search
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

        private IReadOnlyCollection<GetPersonListItemResponseDto> _getPeopleResult;

        protected IReadOnlyCollection<GetPersonListItemResponseDto> GetPeopleResult
        {
            get
            {
                return _getPeopleResult;
            }
            set
            {
                if (!object.Equals(_getPeopleResult, value))
                {
                    _getPeopleResult = value;
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

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Load();
        }

        protected async Task LoadDataAsync(LoadDataArgs args)
        {
            PageSize = args.Top ?? PageSize;
            PageNumber = args.Skip.HasValue
            ? (args.Skip.Value / PageSize) + 1
            : PageNumber;

            await Load();
        }

        protected async System.Threading.Tasks.Task Load()
        {
            if (string.IsNullOrEmpty(search))
            {
                search = string.Empty;
            }

            try
            {
                var localDbGetPeopleResult = await PersonHttpClient.GetListAsync(new GetPersonListRequestDto { Page = new PageRequestDto(PageNumber, PageSize), SearchString = search }, CancellationToken.None);

                GetPeopleResult = localDbGetPeopleResult.Items;
                TotalCount = localDbGetPeopleResult.TotalCount;
            }
            catch (CustomApiException ex)
            {
                NotificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Summary = ex.Message,
                    Detail = ex.Details.ErrorMessage
                });

                if (ex.Details.StatusCode == System.Net.HttpStatusCode.Unauthorized || ex.Details.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    NavigationManager.NavigateTo("/login");
                }
            }
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddPerson>("Add Person", null);

            if ((dialogResult as bool?).GetValueOrDefault())
            {
                await Load();
                await grid0.Reload();
                await InvokeAsync(() => { StateHasChanged(); });
            }
        }

        protected async Task Grid0RowSelect(GetPersonListItemResponseDto args)
        {
            var dialogResult = await DialogService.OpenAsync<EditPerson>("Edit Person", new Dictionary<string, object>() { {"Id", args.Id} });

            if ((dialogResult as bool?).GetValueOrDefault())
            {
                await Load();
                await InvokeAsync(() => { StateHasChanged(); });
            }
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    await PersonHttpClient.DeleteAsync(data.Id, CancellationToken.None);

                    await Load();
                    await grid0.Reload();
                }
            }
            catch (CustomApiException ex)
            {
                NotificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Summary = ex.Message,
                    Detail = ex.Details.ErrorMessage
                });

                if (ex.Details.StatusCode == System.Net.HttpStatusCode.Unauthorized || ex.Details.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    NavigationManager.NavigateTo("/login");
                }
            }
        }
    }
}

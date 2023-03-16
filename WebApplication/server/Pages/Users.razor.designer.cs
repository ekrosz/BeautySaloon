using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using BeautySaloon.Api.Dto.Responses.User;
using BeautySaloon.Api.Services;
using BeautySaloon.Api.Dto.Requests.User;
using WebApplication.Handlers;

namespace WebApplication.Pages
{
    public partial class UsersComponent : ComponentBase
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
        protected IUserHttpClient UserHttpClient { get; set; }

        protected RadzenDataGrid<GetUserResponseDto> grid0;

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

        IReadOnlyCollection<GetUserResponseDto> _getUsersResult;

        protected IReadOnlyCollection<GetUserResponseDto> GetUsersResult
        {
            get
            {
                return _getUsersResult;
            }
            set
            {
                if (!object.Equals(_getUsersResult, value))
                {
                    _getUsersResult = value;
                    Reload();
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Search = string.Empty;
            }

            try
            {
                var localDbGetUsersResult = await UserHttpClient.GetListAsync(new GetUserListRequestDto(Search), CancellationToken.None);

                GetUsersResult = localDbGetUsersResult.Items;
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

        protected async Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddUser>("Создание пользователя", null);

            if ((dialogResult as bool?).GetValueOrDefault())
            {
                await Load();
                await grid0.Reload();
                await InvokeAsync(() => { StateHasChanged(); });
            }
        }

        protected async Task Grid0RowSelect(GetUserResponseDto args)
        {
            var dialogResult = await DialogService.OpenAsync<EditUser>("Редактирование пользователя", new Dictionary<string, object>() { {"Id", args.Id} });

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
                    await UserHttpClient.DeleteAsync(data.Id, CancellationToken.None);

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

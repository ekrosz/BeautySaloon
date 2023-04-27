using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using BeautySaloon.Api.Services;
using WebApplication.Wrappers;
using BeautySaloon.Api.Dto.Responses.User;

namespace WebApplication.Layouts
{
    public partial class MainLayoutComponent : LayoutComponentBase
    {
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

        [Inject]
        protected IHttpClientWrapper HttpClientWrapper { get; set; }

        protected GetUserResponseDto? User { get; set; }

        protected RadzenBody body0;

        protected RadzenSidebar sidebar0;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    var user = await HttpClientWrapper.SendAsync((accessToken) => UserHttpClient.GetAsync(accessToken, CancellationToken.None));

                    if (user == default)
                    {
                        return;
                    }

                    User = user;
                }
                finally
                {
                    StateHasChanged();
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        protected Task OnLogout(dynamic args)
        {
            NavigationManager.NavigateTo("logout");

            return Task.CompletedTask;
        }

        protected async Task SidebarToggle0Click(dynamic args)
        {
            await InvokeAsync(() => { sidebar0.Toggle(); });

            await InvokeAsync(() => { body0.Toggle(); });
        }
    }
}

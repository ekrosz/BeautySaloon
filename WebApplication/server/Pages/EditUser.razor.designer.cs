using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using WebApplication.Services;
using BeautySaloon.Api.Services;
using AutoMapper;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects;
using BeautySaloon.Api.Dto.Requests.User;
using WebApplication.Handlers;
using WebApplication.Wrappers;

namespace WebApplication.Pages
{
    public partial class EditUserComponent : ComponentBase
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
        protected IUserHttpClient UserHttpClient { get; set; }

        [Inject]
        protected IHttpClientWrapper HttpClientWrapper { get; set; }

        [Inject]
        protected IMapper Mapper { get; set; }

        [Parameter]
        public Guid Id { get; set; }

        private UserRequest _user;

        protected UserRequest User
        {
            get
            {
                return _user;
            }
            set
            {
                if (!object.Equals(_user, value))
                {
                    _user = value;
                    Reload();
                }
            }
        }

        protected static IDictionary<Role, string> Roles = Enum.GetValues<Role>()
            .ToDictionary(k => k, v => v switch
            {
                Role.Admin => "Админ",
                Role.Employee => "Сотрудник",
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
        protected async Task Load()
        {
            var user = await HttpClientWrapper.SendAsync((accessToken) => UserHttpClient.GetAsync(accessToken, Id, CancellationToken.None));

            if (user == default)
            {
                return;
            }

            User = Mapper.Map<UserRequest>(user);
        }

        protected async Task Form0Submit(UserRequest args)
        {
            var request = Mapper.Map<UpdateUserRequestDto>(User);

            await HttpClientWrapper.SendAsync((accessToken) => UserHttpClient.UpdateAsync(accessToken, Id, request, CancellationToken.None));

            DialogService.Close(true);
        }

        protected async Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(false);
        }

        public record UserRequest
        {
            public Role Role { get; set; } = Role.Employee;

            public string Login { get; set; } = string.Empty;

            public string Password { get; set; } = string.Empty;

            public string PhoneNumber { get; set; } = string.Empty;

            public string? Email { get; set; }

            public FullName Name { get; set; } = FullName.Empty;
        }
    }
}

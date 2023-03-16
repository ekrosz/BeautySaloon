using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects;
using BeautySaloon.Api.Services;
using AutoMapper;
using BeautySaloon.Api.Dto.Requests.User;
using WebApplication.Handlers;

namespace WebApplication.Pages
{
    public partial class AddUserComponent : ComponentBase
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

        [Inject]
        protected IMapper Mapper { get; set; }

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

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {
            User = new UserRequest();
        }

        protected async Task Form0Submit(UserRequest args)
        {
            try
            {
                var request = Mapper.Map<CreateUserRequestDto>(User);

                await UserHttpClient.CreateAsync(request, CancellationToken.None);

                DialogService.Close(true);
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

using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using BeautySaloon.DAL.Entities.ValueObjects;
using BeautySaloon.Api.Services;
using AutoMapper;
using BeautySaloon.Api.Dto.Requests.Person;
using WebApplication.Handlers;

namespace WebApplication.Pages
{
    public partial class EditPersonComponent : ComponentBase
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
        protected IPersonHttpClient PersonHttpClient { get; set; }

        [Inject]
        protected IMapper Mapper { get; set; }

        [Parameter]
        public Guid Id { get; set; }

        private PersonRequest _person;

        protected PersonRequest Person
        {
            get
            {
                return _person;
            }
            set
            {
                if (!object.Equals(_person, value))
                {
                    _person = value;
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
            try
            {
                var person = await PersonHttpClient.GetAsync(Id, CancellationToken.None);

                Person = Mapper.Map<PersonRequest>(person);
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

        protected async Task Form0Submit(PersonRequest args)
        {
            try
            {
                var request = Mapper.Map<UpdatePersonRequestDto>(Person);

                await PersonHttpClient.UpdateAsync(Id, request, CancellationToken.None);

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

        public record PersonRequest
        {
            public FullName Name { get; set; } = FullName.Empty;

            public DateTime BirthDate { get; set; } = new DateTime(1990, 1, 1);

            public string? Email { get; set; }

            public string PhoneNumber { get; set; } = string.Empty;
        }
    }
}

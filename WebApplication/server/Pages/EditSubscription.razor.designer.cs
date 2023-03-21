using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using BeautySaloon.Api.Services;
using BeautySaloon.Api.Dto.Requests.Subscription;
using AutoMapper;
using WebApplication.Handlers;

namespace WebApplication.Pages
{
    public partial class EditSubscriptionComponent : ComponentBase
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
        protected IMapper Mapper { get; set; }

        [Parameter]
        public dynamic Id { get; set; }

        private SubscriptionRequest _subscription;

        protected SubscriptionRequest Subscription
        {
            get
            {
                return _subscription;
            }
            set
            {
                if (!object.Equals(_subscription, value))
                {
                    _subscription = value;
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
                var subscription = await SubscriptionHttpClient.GetAsync(Guid.Parse(Id), CancellationToken.None);

                Subscription = Mapper.Map<SubscriptionRequest>(subscription);
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

        protected async Task Form0Submit(SubscriptionRequest args)
        {
            try
            {
                var request = Mapper.Map<UpdateSubscriptionRequestDto>(Subscription);

                await SubscriptionHttpClient.UpdateAsync(Id, request, CancellationToken.None);

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

        public record SubscriptionRequest
        {
            public string Name { get; set; } = string.Empty;

            public int? LifeTimeInDays { get; set; }

            public decimal Price { get; set; }

            public IReadOnlyCollection<CosmeticServiceRequest> CosmeticServices { get; set; } = Array.Empty<CosmeticServiceRequest>();

            public record CosmeticServiceRequest
            {
                public Guid Id { get; init; }

                public string Name { get; init; } = string.Empty;

                public string Description { get; init; } = string.Empty;

                public int ExecuteTimeInMinutes { get; init; }

                public int Count { get; init; }
            }
        }
    }
}

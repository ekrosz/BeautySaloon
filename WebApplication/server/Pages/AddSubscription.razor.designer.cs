using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using AutoMapper;
using BeautySaloon.Api.Dto.Requests.Subscription;
using BeautySaloon.Api.Services;
using WebApplication.Wrappers;

namespace WebApplication.Pages
{
    public partial class AddSubscriptionComponent : ComponentBase
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

        [Inject]
        protected IMapper Mapper { get; set; }

        [Parameter]
        public dynamic Id { get; set; }

        private SubscriptionRequest _subscription;

        protected SubscriptionRequest Subscription
        {
            get
            {
                if (_subscription != null)
                {
                    _subscription.CosmeticServices = _subscription.CosmeticServices.OrderBy(x => x.Name).ToList();
                }

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

        protected RadzenDataGrid<SubscriptionRequest.CosmeticServiceRequest> grid0;

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
            Subscription = new SubscriptionRequest();
        }

        protected async Task Form0Submit(SubscriptionRequest args)
        {
            var request = Mapper.Map<CreateSubscriptionRequestDto>(Subscription);

            var isSuccess = await HttpClientWrapper.SendAsync((accessToken) => SubscriptionHttpClient.CreateAsync(accessToken, request, CancellationToken.None));

            if (!isSuccess)
            {
                return;
            }

            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Success,
                Summary = "Запись успешно сохранена"
            });

            NavigationManager.NavigateTo("/subscriptions");
        }

        protected async Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddOrEditSubscriptionCosmeticService>("Добавление услуги в абонемент", null);

            if (dialogResult == null)
            {
                return;
            }

            var cosmeticService = Mapper.Map<SubscriptionRequest.CosmeticServiceRequest>((AddOrEditSubscriptionCosmeticServiceComponent.SubscriptionCosmeticServiceRequest)dialogResult);

            var existingCosmeticService = Subscription.CosmeticServices.FirstOrDefault(x => x.Id == cosmeticService.Id);

            if (existingCosmeticService != null)
            {
                existingCosmeticService.Count += cosmeticService.Count;
            }
            else
            {
                Subscription.CosmeticServices.Add(cosmeticService);
            }

            await grid0.Reload();
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async Task Grid0RowSelect(SubscriptionRequest.CosmeticServiceRequest args)
        {
            var parameter = Mapper.Map<AddOrEditSubscriptionCosmeticServiceComponent.SubscriptionCosmeticServiceRequest>(args);

            var dialogResult = await DialogService.OpenAsync<AddOrEditSubscriptionCosmeticService>("Редактирование услуги абонемента", new Dictionary<string, object>() { { "SubscriptionCosmeticService", parameter } });

            if (dialogResult == null)
            {
                return;
            }

            var cosmeticService = (AddOrEditSubscriptionCosmeticServiceComponent.SubscriptionCosmeticServiceRequest)dialogResult;

            var existingCosmeticService = Subscription.CosmeticServices.First(x => x.Id == cosmeticService.Id);

            Mapper.Map(cosmeticService, existingCosmeticService);

            await grid0.Reload();
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected Task Button2Click(MouseEventArgs args)
        {
            NavigationManager.NavigateTo("/subscriptions");

            return Task.CompletedTask;
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
            {
                var cosmeticService = Subscription.CosmeticServices.First(x => x.Id == data.Id);

                Subscription.CosmeticServices.Remove(cosmeticService);

                await grid0.Reload();
            }
        }

        public record SubscriptionRequest
        {
            public string Name { get; set; } = string.Empty;

            public int? LifeTimeInDays { get; set; }

            public decimal Price { get; set; }

            public List<CosmeticServiceRequest> CosmeticServices { get; set; } = new List<CosmeticServiceRequest>();

            public record CosmeticServiceRequest
            {
                public Guid Id { get; set; }

                public string Name { get; set; } = string.Empty;

                public int Count { get; set; }
            }
        }
    }
}

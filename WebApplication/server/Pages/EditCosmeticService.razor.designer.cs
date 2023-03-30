using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using BeautySaloon.Api.Services;
using AutoMapper;
using BeautySaloon.Api.Dto.Requests.CosmeticService;
using WebApplication.Wrappers;

namespace WebApplication.Pages
{
    public partial class EditCosmeticServiceComponent : ComponentBase
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
        protected ICosmeticServiceHttpClient CosmeticServiceHttpClient { get; set; }

        [Inject]
        protected IHttpClientWrapper HttpClientWrapper { get; set; }

        [Inject]
        protected IMapper Mapper { get; set; }

        [Parameter]
        public Guid Id { get; set; }

        private CosmeticServiceRequest _cosmeticService;

        protected CosmeticServiceRequest CosmeticService
        {
            get
            {
                return _cosmeticService;
            }
            set
            {
                if (!object.Equals(_cosmeticService, value))
                {
                    _cosmeticService = value;
                    Reload();
                }
            }
        }

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
            var cosmeticService = await HttpClientWrapper.SendAsync((accessToken) => CosmeticServiceHttpClient.GetAsync(accessToken, Id, CancellationToken.None));

            if (cosmeticService == default)
            {
                return;
            }

            CosmeticService = Mapper.Map<CosmeticServiceRequest>(cosmeticService);
        }

        protected async Task Form0Submit(CosmeticServiceRequest args)
        {
            var request = Mapper.Map<UpdateCosmeticServiceRequestDto>(CosmeticService);

            var isSuccess = await HttpClientWrapper.SendAsync((accessToken) => CosmeticServiceHttpClient.UpdateAsync(accessToken, Id, request, CancellationToken.None));

            DialogService.Close(isSuccess);
        }

        protected async Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        public record CosmeticServiceRequest
        {
            public string Name { get; set; } = string.Empty;

            public string Description { get; set; } = string.Empty;

            public int ExecuteTimeInMinutes { get; set; }
        }
    }
}

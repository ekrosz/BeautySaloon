using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using BeautySaloon.Api.Dto.Requests.CosmeticService;
using BeautySaloon.Api.Services;
using AutoMapper;
using WebApplication.Handlers;
using WebApplication.Wrappers;

namespace WebApplication.Pages
{
    public partial class AddCosmeticServiceComponent : ComponentBase
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

        private CosmeticServiceRequest _cosmeticservice;

        protected CosmeticServiceRequest CosmeticService
        {
            get
            {
                return _cosmeticservice;
            }
            set
            {
                if (!object.Equals(_cosmeticservice, value))
                {
                    _cosmeticservice = value;
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
            CosmeticService = new CosmeticServiceRequest(){};
        }

        protected async Task Form0Submit(CosmeticServiceRequest args)
        {
            var request = Mapper.Map<CreateCosmeticServiceRequestDto>(CosmeticService);

            await HttpClientWrapper.SendAsync((accessToken) => CosmeticServiceHttpClient.CreateAsync(accessToken, request, CancellationToken.None));

            DialogService.Close(true);
        }

        protected async Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(false);
        }

        public record CosmeticServiceRequest
        {
            public string Name { get; set; } = string.Empty;

            public string Description { get; set; } = string.Empty;

            public int ExecuteTimeInMinutes { get; set; }
        }
    }
}

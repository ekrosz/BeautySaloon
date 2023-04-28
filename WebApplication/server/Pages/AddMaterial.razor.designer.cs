using AutoMapper;
using BeautySaloon.Api.Dto.Requests.Material;
using BeautySaloon.Api.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using WebApplication.Wrappers;

namespace WebApplication.Pages
{
    public partial class AddMaterialComponent : ComponentBase
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
        protected IMaterialHttpClient MaterialHttpClient { get; set; }

        [Inject]
        protected IHttpClientWrapper HttpClientWrapper { get; set; }

        [Inject]
        protected IMapper Mapper { get; set; }

        private MaterialRequest _material;

        protected MaterialRequest Material
        {
            get
            {
                return _material;
            }
            set
            {
                if (!object.Equals(_material, value))
                {
                    _material = value;
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
            Material = new MaterialRequest() { };
        }

        protected async Task Form0Submit(MaterialRequest args)
        {
            var request = Mapper.Map<CreateMaterialRequestDto>(Material);

            var isSuccess = await HttpClientWrapper.SendAsync((accessToken) => MaterialHttpClient.CreateAsync(accessToken, request, CancellationToken.None));

            DialogService.Close(isSuccess);
        }

        protected async Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(false);
        }

        public record MaterialRequest
        {
            public string Name { get; set; } = string.Empty;

            public string? Description { get; set; }

        }
    }
}

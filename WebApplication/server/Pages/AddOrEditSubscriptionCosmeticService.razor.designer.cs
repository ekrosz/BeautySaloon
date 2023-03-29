using BeautySaloon.Api.Dto.Requests.CosmeticService;
using BeautySaloon.Api.Dto.Responses.CosmeticService;
using BeautySaloon.Api.Services;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using WebApplication.Wrappers;

namespace WebApplication.Pages;

public partial class AddOrEditSubscriptionCosmeticServiceComponent : ComponentBase
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

    private SubscriptionCosmeticServiceRequest _subscriptionCosmeticService;

    [Parameter]
    public SubscriptionCosmeticServiceRequest SubscriptionCosmeticService
    {
        get
        {
            return _subscriptionCosmeticService;
        }
        set
        {
            if (!object.Equals(_subscriptionCosmeticService, value))
            {
                _subscriptionCosmeticService = value;
                Reload();
            }
        }
    }

    private IReadOnlyCollection<GetCosmeticServiceResponseDto> _cosmeticServices;

    protected IReadOnlyCollection<GetCosmeticServiceResponseDto> CosmeticServices
    {
        get
        {
            return _cosmeticServices;
        }
        set
        {
            if (!object.Equals(_cosmeticServices, value))
            {
                _cosmeticServices = value;
                Reload();
            }
        }
    }

    protected bool IsDropDownDisabled { get; set; }

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
        if (SubscriptionCosmeticService == null)
        {
            SubscriptionCosmeticService = new SubscriptionCosmeticServiceRequest();
            IsDropDownDisabled = false;
        }
        else
        {
            IsDropDownDisabled = true;
        }

        CosmeticServices = await GetCosmeticServicesAsync();
    }

    protected async Task Form0Submit(SubscriptionCosmeticServiceRequest args)
    {
        args.Name = CosmeticServices.First(x => x.Id == args.Id).Name;

        DialogService.Close(args);
    }

    protected async Task Button2Click(MouseEventArgs args)
    {
        DialogService.Close(null);
    }

    private async Task<IReadOnlyCollection<GetCosmeticServiceResponseDto>> GetCosmeticServicesAsync()
    {
        var pageSize = 100;
        var pageNumber = 1;
        var totalCount = 1;

        async Task<PageResponseDto<GetCosmeticServiceResponseDto>?> GetListAsync(int number, int size)
        {
            var cosmeticServices = await HttpClientWrapper.SendAsync((accessToken)
                => CosmeticServiceHttpClient.GetListAsync(accessToken, new GetCosmeticServiceListRequestDto { Page = new PageRequestDto(number, size) }, CancellationToken.None));

            totalCount = cosmeticServices.TotalCount - pageSize;
            pageNumber++;

            return cosmeticServices;
        }

        var result = new List<GetCosmeticServiceResponseDto>();

        while (totalCount > 0)
        {
            var cosmeticServices = await GetListAsync(pageNumber, pageSize);

            if (cosmeticServices != default)
            {
                result.AddRange(cosmeticServices.Items);
            }
        }

        return result.ToArray();
    }

    public record SubscriptionCosmeticServiceRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Count { get; set; }
    }
}

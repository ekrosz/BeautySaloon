using BeautySaloon.Api.Dto.Requests.CosmeticService;
using BeautySaloon.Api.Dto.Responses.CosmeticService;
using BeautySaloon.Api.Services;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;

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

    protected override async Task OnInitializedAsync()
    {
        await Load();
    }
    protected async Task Load()
    {
        if (SubscriptionCosmeticService == null)
        {
            SubscriptionCosmeticService = new SubscriptionCosmeticServiceRequest();
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
        var pageNumber = 1;
        var pageSize = 100;
        var totalCount = 100;

        var result = new List<GetCosmeticServiceResponseDto>();

        var cosmeticServices = await CosmeticServiceHttpClient.GetListAsync(new GetCosmeticServiceListRequestDto { Page = new PageRequestDto(pageNumber, pageSize) }, CancellationToken.None);

        result.AddRange(cosmeticServices.Items);
        totalCount = cosmeticServices.TotalCount - pageSize;
        pageNumber++;

        while (totalCount > 0)
        {
            cosmeticServices = await CosmeticServiceHttpClient.GetListAsync(new GetCosmeticServiceListRequestDto { Page = new PageRequestDto(pageNumber, pageSize) }, CancellationToken.None);

            result.AddRange(cosmeticServices.Items);
            totalCount = cosmeticServices.TotalCount - totalCount;
            pageNumber++;
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

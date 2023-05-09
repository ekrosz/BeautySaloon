using BeautySaloon.Api.Dto.Requests.Material;
using BeautySaloon.Api.Dto.Responses.Material;
using BeautySaloon.Api.Services;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using WebApplication.Wrappers;

namespace WebApplication.Pages;

public partial class AddOrEditInvoiceMaterialComponent : ComponentBase
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

    private InvoiceMaterialRequest _invoiceMaterial;

    [Parameter]
    public InvoiceType InvoiceType { get; set; }

    [Parameter]
    public InvoiceMaterialRequest InvoiceMaterial
    {
        get
        {
            return _invoiceMaterial;
        }
        set
        {
            if (!object.Equals(_invoiceMaterial, value))
            {
                _invoiceMaterial = value;
                Reload();
            }
        }
    }

    private IReadOnlyCollection<GetMaterialResponseDto> _materials;

    protected IReadOnlyCollection<GetMaterialResponseDto> Materials
    {
        get
        {
            return _materials;
        }
        set
        {
            if (!object.Equals(_materials, value))
            {
                _materials = value;
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
        if (InvoiceMaterial == null)
        {
            InvoiceMaterial = new InvoiceMaterialRequest();
            IsDropDownDisabled = false;
        }
        else
        {
            IsDropDownDisabled = true;
        }

        Materials = await GetMaterialsAsync();
    }

    protected async Task Form0Submit(InvoiceMaterialRequest args)
    {
        args.Name = Materials.First(x => x.Id == args.Id).Name;

        DialogService.Close(args);
    }

    protected async Task Button2Click(MouseEventArgs args)
    {
        DialogService.Close(null);
    }

    private async Task<IReadOnlyCollection<GetMaterialResponseDto>> GetMaterialsAsync()
    {
        var pageSize = 100;
        var pageNumber = 1;
        var totalCount = 1;

        async Task<PageResponseDto<GetMaterialResponseDto>?> GetListAsync(int number, int size)
        {
            var materials = await HttpClientWrapper.SendAsync((accessToken)
                => MaterialHttpClient.GetListAsync(accessToken, new GetMaterialListRequestDto { Page = new PageRequestDto(number, size) }, CancellationToken.None));

            totalCount = materials.TotalCount - pageSize;
            pageNumber++;

            return materials;
        }

        var result = new List<GetMaterialResponseDto>();

        while (totalCount > 0)
        {
            var materials = await GetListAsync(pageNumber, pageSize);

            if (materials != default)
            {
                result.AddRange(materials.Items);
            }
        }

        return result.ToArray();
    }

    public record InvoiceMaterialRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Count { get; set; }

        public decimal? Cost { get; set; }
    }
}

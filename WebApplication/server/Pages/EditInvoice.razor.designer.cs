using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using AutoMapper;
using BeautySaloon.Api.Dto.Requests.Invoice;
using BeautySaloon.Api.Services;
using WebApplication.Wrappers;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects;
using BeautySaloon.Api.Dto.Responses.User;
using BeautySaloon.Api.Dto.Responses.Invoice;
using BeautySaloon.Api.Dto.Requests.User;

namespace WebApplication.Pages
{
    public partial class EditInvoiceComponent : ComponentBase
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
        protected IInvoiceHttpClient InvoiceHttpClient { get; set; }

        [Inject]
        protected IUserHttpClient UserHttpClient { get; set; }

        [Inject]
        protected IHttpClientWrapper HttpClientWrapper { get; set; }

        [Inject]
        protected IMapper Mapper { get; set; }

        [Parameter]
        public dynamic Id { get; set; }

        private InvoiceRequest _invoice;

        protected InvoiceRequest Invoice
        {
            get
            {
                if (_invoice != null)
                {
                    _invoice.Materials = _invoice.Materials.OrderBy(x => x.Name).ToList();
                }

                return _invoice;
            }
            set
            {
                if (!object.Equals(_invoice, value))
                {
                    _invoice = value;
                    Reload();
                }
            }
        }

        private IReadOnlyCollection<GetUserResponseDto> _getUsersResult;

        protected IReadOnlyCollection<GetUserResponseDto> GetUsersResult
        {
            get
            {
                return _getUsersResult;
            }
            set
            {
                if (!object.Equals(_getUsersResult, value))
                {
                    _getUsersResult = value;
                    Reload();
                }
            }
        }

        protected RadzenDataGrid<InvoiceRequest.MaterialRequest> grid0;

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
            var invoice = await HttpClientWrapper.SendAsync<GetInvoiceResponseDto>((accessToken) => InvoiceHttpClient.GetAsync(accessToken, Guid.Parse(Id), CancellationToken.None));

            if (invoice == default)
            {
                return;
            }

            var employees = await HttpClientWrapper.SendAsync((accessToken) => UserHttpClient.GetListAsync(accessToken, new GetUserListRequestDto(null), CancellationToken.None));

            if (employees == default)
            {
                return;
            }

            Invoice = Mapper.Map<InvoiceRequest>(invoice);

            GetUsersResult = employees.Items;
        }

        protected async Task Form0Submit(InvoiceRequest args)
        {
            var request = Mapper.Map<UpdateInvoiceRequestDto>(Invoice);

            var isSuccess = await HttpClientWrapper.SendAsync((accessToken) => InvoiceHttpClient.UpdateAsync(accessToken, Guid.Parse(Id), request, CancellationToken.None));

            if (!isSuccess)
            {
                return;
            }

            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Success,
                Summary = "Запись успешно сохранена"
            });

            NavigationManager.NavigateTo("/invoices");
        }

        protected static IDictionary<InvoiceType, string> InvoiceTypes = Enum.GetValues<InvoiceType>()
           .ToDictionary(k => k, v => v switch
           {
               InvoiceType.Extradition => "Расход",
               InvoiceType.Receipt => "Приход",
               _ => string.Empty
           });


        protected async Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddOrEditInvoiceMaterial>("Добавить движение", new Dictionary<string, object>() { { "InvoiceType", Invoice.InvoiceType } });

            if (dialogResult == null)
            {
                return;
            }

            var material = Mapper.Map<InvoiceRequest.MaterialRequest>((AddOrEditInvoiceMaterialComponent.InvoiceMaterialRequest)dialogResult);

            var existingMaterial = Invoice.Materials.FirstOrDefault(x => x.Id == material.Id);

            if (existingMaterial != null)
            {
                existingMaterial.Count += material.Count;
                existingMaterial.Cost += material.Cost;
            }
            else
            {
                Invoice.Materials.Add(material);
            }

            await grid0.Reload();
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async Task Grid0RowSelect(InvoiceRequest.MaterialRequest args)
        {
            var parameter = Mapper.Map<AddOrEditInvoiceMaterialComponent.InvoiceMaterialRequest>(args);

            var dialogResult = await DialogService.OpenAsync<AddOrEditInvoiceMaterial>("Редактирование движение", new Dictionary<string, object>() { { "InvoiceMaterial", parameter }, { "InvoiceType", Invoice.InvoiceType } });

            if (dialogResult == null)
            {
                return;
            }

            var material = (AddOrEditInvoiceMaterialComponent.InvoiceMaterialRequest)dialogResult;

            var existingMaterial = Invoice.Materials.First(x => x.Id == material.Id);

            Mapper.Map(material, existingMaterial);

            await grid0.Reload();
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected Task Button2Click(MouseEventArgs args)
        {
            NavigationManager.NavigateTo("/invoices");

            return Task.CompletedTask;
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            if (await DialogService.Confirm("Вы действительно хотите удалить запись?") == true)
            {
                var cosmeticService = Invoice.Materials.First(x => x.Id == data.Id);

                Invoice.Materials.Remove(cosmeticService);

                await grid0.Reload();
            }
        }

        protected async Task OnInvoiceTypeSelectedEvent(object args)
        {
            if (((InvoiceType)args) == InvoiceType.Receipt)
            {
                Invoice.EmployeeId = null;
            }
        }

        public record InvoiceRequest
        {
            public InvoiceType InvoiceType { get; set; } = InvoiceType.Receipt;

            public DateTime InvoiceDate { get; set; } = DateTime.Now;

            public string? Comment { get; set; }

            public Guid? EmployeeId { get; set; }

            public List<MaterialRequest> Materials { get; set; } = new List<MaterialRequest>();

            public record MaterialRequest
            {
                public Guid Id { get; set; }

                public string Name { get; set; } = string.Empty;

                public int Count { get; set; }

                public decimal? Cost { get; set; }
            }
        }
    }
}

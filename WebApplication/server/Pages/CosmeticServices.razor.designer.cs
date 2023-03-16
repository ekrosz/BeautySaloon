using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using WebApplication.Models.LocalDb;
using Microsoft.EntityFrameworkCore;
using WebApplication.Services;

namespace WebApplication.Pages
{
    public partial class CosmeticServicesComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected LocalDbService LocalDb { get; set; }
        protected RadzenDataGrid<WebApplication.Models.LocalDb.CosmeticService> grid0;

        string _search;
        protected string search
        {
            get
            {
                return _search;
            }
            set
            {
                if (!object.Equals(_search, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "search", NewValue = value, OldValue = _search };
                    _search = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<WebApplication.Models.LocalDb.CosmeticService> _getCosmeticServicesResult;
        protected IEnumerable<WebApplication.Models.LocalDb.CosmeticService> getCosmeticServicesResult
        {
            get
            {
                return _getCosmeticServicesResult;
            }
            set
            {
                if (!object.Equals(_getCosmeticServicesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getCosmeticServicesResult", NewValue = value, OldValue = _getCosmeticServicesResult };
                    _getCosmeticServicesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            if (string.IsNullOrEmpty(search)) {
                search = "";
            }

            var localDbGetCosmeticServicesResult = await LocalDb.GetCosmeticServices(new Query() { Filter = $@"i => i.Name.Contains(@0) || i.Description.Contains(@1)", FilterParameters = new object[] { search, search } });
            getCosmeticServicesResult = localDbGetCosmeticServicesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddCosmeticService>("Add Cosmetic Service", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(WebApplication.Models.LocalDb.CosmeticService args)
        {
            var dialogResult = await DialogService.OpenAsync<EditCosmeticService>("Edit Cosmetic Service", new Dictionary<string, object>() { {"Id", args.Id} });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var localDbDeleteCosmeticServiceResult = await LocalDb.DeleteCosmeticService(data.Id);
                    if (localDbDeleteCosmeticServiceResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception localDbDeleteCosmeticServiceException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete CosmeticService" });
            }
        }
    }
}

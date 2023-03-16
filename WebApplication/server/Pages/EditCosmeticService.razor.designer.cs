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
    public partial class EditCosmeticServiceComponent : ComponentBase
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

        [Parameter]
        public dynamic Id { get; set; }

        WebApplication.Models.LocalDb.CosmeticService _cosmeticservice;
        protected WebApplication.Models.LocalDb.CosmeticService cosmeticservice
        {
            get
            {
                return _cosmeticservice;
            }
            set
            {
                if (!object.Equals(_cosmeticservice, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "cosmeticservice", NewValue = value, OldValue = _cosmeticservice };
                    _cosmeticservice = value;
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
            var localDbGetCosmeticServiceByIdResult = await LocalDb.GetCosmeticServiceById(Id);
            cosmeticservice = localDbGetCosmeticServiceByIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(WebApplication.Models.LocalDb.CosmeticService args)
        {
            try
            {
                var localDbUpdateCosmeticServiceResult = await LocalDb.UpdateCosmeticService(Id, cosmeticservice);
                DialogService.Close(cosmeticservice);
            }
            catch (System.Exception localDbUpdateCosmeticServiceException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to update CosmeticService" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}

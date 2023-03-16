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
    public partial class SubscriptionsComponent : ComponentBase
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
        protected RadzenDataGrid<WebApplication.Models.LocalDb.Subscription> grid0;

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

        IEnumerable<WebApplication.Models.LocalDb.Subscription> _getSubscriptionsResult;
        protected IEnumerable<WebApplication.Models.LocalDb.Subscription> getSubscriptionsResult
        {
            get
            {
                return _getSubscriptionsResult;
            }
            set
            {
                if (!object.Equals(_getSubscriptionsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getSubscriptionsResult", NewValue = value, OldValue = _getSubscriptionsResult };
                    _getSubscriptionsResult = value;
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

            var localDbGetSubscriptionsResult = await LocalDb.GetSubscriptions(new Query() { Filter = $@"i => i.Name.Contains(@0)", FilterParameters = new object[] { search } });
            getSubscriptionsResult = localDbGetSubscriptionsResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddSubscription>("Add Subscription", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(WebApplication.Models.LocalDb.Subscription args)
        {
            var dialogResult = await DialogService.OpenAsync<EditSubscription>("Edit Subscription", new Dictionary<string, object>() { {"Id", args.Id} });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var localDbDeleteSubscriptionResult = await LocalDb.DeleteSubscription(data.Id);
                    if (localDbDeleteSubscriptionResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception localDbDeleteSubscriptionException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Subscription" });
            }
        }
    }
}

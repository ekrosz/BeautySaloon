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
    public partial class EditSubscriptionComponent : ComponentBase
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

        WebApplication.Models.LocalDb.Subscription _subscription;
        protected WebApplication.Models.LocalDb.Subscription subscription
        {
            get
            {
                return _subscription;
            }
            set
            {
                if (!object.Equals(_subscription, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "subscription", NewValue = value, OldValue = _subscription };
                    _subscription = value;
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
            var localDbGetSubscriptionByIdResult = await LocalDb.GetSubscriptionById(Id);
            subscription = localDbGetSubscriptionByIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(WebApplication.Models.LocalDb.Subscription args)
        {
            try
            {
                var localDbUpdateSubscriptionResult = await LocalDb.UpdateSubscription(Id, subscription);
                DialogService.Close(subscription);
            }
            catch (System.Exception localDbUpdateSubscriptionException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to update Subscription" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}

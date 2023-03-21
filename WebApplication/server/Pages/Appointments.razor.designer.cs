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
    public partial class AppointmentsComponent : ComponentBase
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
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected LocalDbService LocalDb { get; set; }
        protected RadzenDataGrid<WebApplication.Models.LocalDb.Appointment> grid0;

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

        IEnumerable<WebApplication.Models.LocalDb.Appointment> _getAppointmentsResult;
        protected IEnumerable<WebApplication.Models.LocalDb.Appointment> getAppointmentsResult
        {
            get
            {
                return _getAppointmentsResult;
            }
            set
            {
                if (!object.Equals(_getAppointmentsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getAppointmentsResult", NewValue = value, OldValue = _getAppointmentsResult };
                    _getAppointmentsResult = value;
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

            var localDbGetAppointmentsResult = await LocalDb.GetAppointments(new Query() { Filter = $@"i => i.Comment.Contains(@0)", FilterParameters = new object[] { search }, Expand = "User,Person" });
            getAppointmentsResult = localDbGetAppointmentsResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddAppointment>("Add Appointment", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(WebApplication.Models.LocalDb.Appointment args)
        {
            var dialogResult = await DialogService.OpenAsync<EditAppointment>("Edit Appointment", new Dictionary<string, object>() { {"Id", args.Id} });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var localDbDeleteAppointmentResult = await LocalDb.DeleteAppointment(data.Id);
                    if (localDbDeleteAppointmentResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception localDbDeleteAppointmentException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Appointment" });
            }
        }
    }
}

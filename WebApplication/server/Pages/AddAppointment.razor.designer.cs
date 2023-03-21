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
    public partial class AddAppointmentComponent : ComponentBase
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

        IEnumerable<WebApplication.Models.LocalDb.User> _getUsersResult;
        protected IEnumerable<WebApplication.Models.LocalDb.User> getUsersResult
        {
            get
            {
                return _getUsersResult;
            }
            set
            {
                if (!object.Equals(_getUsersResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getUsersResult", NewValue = value, OldValue = _getUsersResult };
                    _getUsersResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<WebApplication.Models.LocalDb.Person> _getPeopleResult;
        protected IEnumerable<WebApplication.Models.LocalDb.Person> getPeopleResult
        {
            get
            {
                return _getPeopleResult;
            }
            set
            {
                if (!object.Equals(_getPeopleResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getPeopleResult", NewValue = value, OldValue = _getPeopleResult };
                    _getPeopleResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        WebApplication.Models.LocalDb.Appointment _appointment;
        protected WebApplication.Models.LocalDb.Appointment appointment
        {
            get
            {
                return _appointment;
            }
            set
            {
                if (!object.Equals(_appointment, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "appointment", NewValue = value, OldValue = _appointment };
                    _appointment = value;
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
            var localDbGetUsersResult = await LocalDb.GetUsers();
            getUsersResult = localDbGetUsersResult;

            var localDbGetPeopleResult = await LocalDb.GetPeople();
            getPeopleResult = localDbGetPeopleResult;

            appointment = new WebApplication.Models.LocalDb.Appointment(){};
        }

        protected async System.Threading.Tasks.Task Form0Submit(WebApplication.Models.LocalDb.Appointment args)
        {
            try
            {
                var localDbCreateAppointmentResult = await LocalDb.CreateAppointment(appointment);
                DialogService.Close(appointment);
            }
            catch (System.Exception localDbCreateAppointmentException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to create new Appointment!" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}

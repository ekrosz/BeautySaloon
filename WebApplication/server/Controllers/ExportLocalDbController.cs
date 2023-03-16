using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    public partial class ExportLocalDbController : ExportController
    {
        private readonly LocalDbContext context;
        private readonly LocalDbService service;
        public ExportLocalDbController(LocalDbContext context, LocalDbService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/LocalDb/appointments/csv")]
        [HttpGet("/export/LocalDb/appointments/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportAppointmentsToCSV(string? fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAppointments(), Request.Query), fileName);
        }

        [HttpGet("/export/LocalDb/appointments/excel")]
        [HttpGet("/export/LocalDb/appointments/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportAppointmentsToExcel(string? fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAppointments(), Request.Query), fileName);
        }
        [HttpGet("/export/LocalDb/cosmeticservices/csv")]
        [HttpGet("/export/LocalDb/cosmeticservices/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportCosmeticServicesToCSV(string? fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCosmeticServices(), Request.Query), fileName);
        }

        [HttpGet("/export/LocalDb/cosmeticservices/excel")]
        [HttpGet("/export/LocalDb/cosmeticservices/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportCosmeticServicesToExcel(string? fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCosmeticServices(), Request.Query), fileName);
        }
        [HttpGet("/export/LocalDb/orders/csv")]
        [HttpGet("/export/LocalDb/orders/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportOrdersToCSV(string? fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetOrders(), Request.Query), fileName);
        }

        [HttpGet("/export/LocalDb/orders/excel")]
        [HttpGet("/export/LocalDb/orders/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportOrdersToExcel(string? fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetOrders(), Request.Query), fileName);
        }
        [HttpGet("/export/LocalDb/people/csv")]
        [HttpGet("/export/LocalDb/people/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportPeopleToCSV(string? fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetPeople(), Request.Query), fileName);
        }

        [HttpGet("/export/LocalDb/people/excel")]
        [HttpGet("/export/LocalDb/people/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportPeopleToExcel(string? fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetPeople(), Request.Query), fileName);
        }
        [HttpGet("/export/LocalDb/subscriptions/csv")]
        [HttpGet("/export/LocalDb/subscriptions/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportSubscriptionsToCSV(string? fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSubscriptions(), Request.Query), fileName);
        }

        [HttpGet("/export/LocalDb/subscriptions/excel")]
        [HttpGet("/export/LocalDb/subscriptions/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportSubscriptionsToExcel(string? fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSubscriptions(), Request.Query), fileName);
        }
        [HttpGet("/export/LocalDb/users/csv")]
        [HttpGet("/export/LocalDb/users/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportUsersToCSV(string? fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetUsers(), Request.Query), fileName);
        }

        [HttpGet("/export/LocalDb/users/excel")]
        [HttpGet("/export/LocalDb/users/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportUsersToExcel(string? fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetUsers(), Request.Query), fileName);
        }
    }
}

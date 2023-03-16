using Radzen;
using System;
using System.Web;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Data;
using System.Text.Encodings.Web;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using WebApplication.Data;

namespace WebApplication.Services
{
    public partial class LocalDbService
    {
        LocalDbContext Context
        {
            get
            {
                return context;
            }
        }

        private readonly LocalDbContext context;
        private readonly NavigationManager navigationManager;

        public LocalDbService(LocalDbContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public async Task ExportAppointmentsToExcel(Query query = null, string? fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/localdb/appointments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/localdb/appointments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAppointmentsToCSV(Query query = null, string? fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/localdb/appointments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/localdb/appointments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAppointmentsRead(ref IQueryable<Models.LocalDb.Appointment> items);

        public async Task<IQueryable<Models.LocalDb.Appointment>> GetAppointments(Query query = null)
        {
            var items = Context.Appointments.AsQueryable();

            items = items.Include(i => i.User);

            items = items.Include(i => i.Person);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnAppointmentsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAppointmentCreated(Models.LocalDb.Appointment item);
        partial void OnAfterAppointmentCreated(Models.LocalDb.Appointment item);

        public async Task<Models.LocalDb.Appointment> CreateAppointment(Models.LocalDb.Appointment appointment)
        {
            OnAppointmentCreated(appointment);

            var existingItem = Context.Appointments
                              .Where(i => i.Id == appointment.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
                throw new Exception("Item already available");
            }

            try
            {
                Context.Appointments.Add(appointment);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(appointment).State = EntityState.Detached;
                appointment.User = null;
                appointment.Person = null;
                throw;
            }

            OnAfterAppointmentCreated(appointment);

            return appointment;
        }
        public Task ExportCosmeticServicesToExcel(Query? query = null, string? fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/localdb/cosmeticservices/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/localdb/cosmeticservices/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
            return Task.CompletedTask;
        }

        public Task ExportCosmeticServicesToCSV(Query query = null, string? fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/localdb/cosmeticservices/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/localdb/cosmeticservices/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
            return Task.CompletedTask;
        }

        partial void OnCosmeticServicesRead(ref IQueryable<Models.LocalDb.CosmeticService> items);

        public async Task<IQueryable<Models.LocalDb.CosmeticService>> GetCosmeticServices(Query query = null)
        {
            var items = Context.CosmeticServices.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnCosmeticServicesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCosmeticServiceCreated(Models.LocalDb.CosmeticService item);
        partial void OnAfterCosmeticServiceCreated(Models.LocalDb.CosmeticService item);

        public async Task<Models.LocalDb.CosmeticService> CreateCosmeticService(Models.LocalDb.CosmeticService cosmeticService)
        {
            OnCosmeticServiceCreated(cosmeticService);

            var existingItem = Context.CosmeticServices
                              .Where(i => i.Id == cosmeticService.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
                throw new Exception("Item already available");
            }

            try
            {
                Context.CosmeticServices.Add(cosmeticService);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(cosmeticService).State = EntityState.Detached;
                throw;
            }

            OnAfterCosmeticServiceCreated(cosmeticService);

            return cosmeticService;
        }
        public async Task ExportOrdersToExcel(Query query = null, string? fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/localdb/orders/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/localdb/orders/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportOrdersToCSV(Query query = null, string? fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/localdb/orders/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/localdb/orders/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnOrdersRead(ref IQueryable<Models.LocalDb.Order> items);

        public async Task<IQueryable<Models.LocalDb.Order>> GetOrders(Query query = null)
        {
            var items = Context.Orders.AsQueryable();

            items = items.Include(i => i.Person);

            items = items.Include(i => i.User);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnOrdersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnOrderCreated(Models.LocalDb.Order item);
        partial void OnAfterOrderCreated(Models.LocalDb.Order item);

        public async Task<Models.LocalDb.Order> CreateOrder(Models.LocalDb.Order order)
        {
            OnOrderCreated(order);

            var existingItem = Context.Orders
                              .Where(i => i.Id == order.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
                throw new Exception("Item already available");
            }

            try
            {
                Context.Orders.Add(order);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(order).State = EntityState.Detached;
                order.Person = null;
                order.User = null;
                throw;
            }

            OnAfterOrderCreated(order);

            return order;
        }
        public async Task ExportPeopleToExcel(Query query = null, string? fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/localdb/people/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/localdb/people/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportPeopleToCSV(Query query = null, string? fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/localdb/people/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/localdb/people/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnPeopleRead(ref IQueryable<Models.LocalDb.Person> items);

        public async Task<IQueryable<Models.LocalDb.Person>> GetPeople(Query query = null)
        {
            var items = Context.People.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnPeopleRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPersonCreated(Models.LocalDb.Person item);
        partial void OnAfterPersonCreated(Models.LocalDb.Person item);

        public async Task<Models.LocalDb.Person> CreatePerson(Models.LocalDb.Person person)
        {
            OnPersonCreated(person);

            var existingItem = Context.People
                              .Where(i => i.Id == person.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
                throw new Exception("Item already available");
            }

            try
            {
                Context.People.Add(person);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(person).State = EntityState.Detached;
                throw;
            }

            OnAfterPersonCreated(person);

            return person;
        }
        public async Task ExportSubscriptionsToExcel(Query query = null, string? fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/localdb/subscriptions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/localdb/subscriptions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSubscriptionsToCSV(Query query = null, string? fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/localdb/subscriptions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/localdb/subscriptions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSubscriptionsRead(ref IQueryable<Models.LocalDb.Subscription> items);

        public async Task<IQueryable<Models.LocalDb.Subscription>> GetSubscriptions(Query query = null)
        {
            var items = Context.Subscriptions.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnSubscriptionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSubscriptionCreated(Models.LocalDb.Subscription item);
        partial void OnAfterSubscriptionCreated(Models.LocalDb.Subscription item);

        public async Task<Models.LocalDb.Subscription> CreateSubscription(Models.LocalDb.Subscription subscription)
        {
            OnSubscriptionCreated(subscription);

            var existingItem = Context.Subscriptions
                              .Where(i => i.Id == subscription.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
                throw new Exception("Item already available");
            }

            try
            {
                Context.Subscriptions.Add(subscription);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(subscription).State = EntityState.Detached;
                throw;
            }

            OnAfterSubscriptionCreated(subscription);

            return subscription;
        }
        public async Task ExportUsersToExcel(Query query = null, string? fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/localdb/users/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/localdb/users/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportUsersToCSV(Query query = null, string? fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/localdb/users/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/localdb/users/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnUsersRead(ref IQueryable<Models.LocalDb.User> items);

        public async Task<IQueryable<Models.LocalDb.User>> GetUsers(Query query = null)
        {
            var items = Context.Users.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnUsersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnUserCreated(Models.LocalDb.User item);
        partial void OnAfterUserCreated(Models.LocalDb.User item);

        public async Task<Models.LocalDb.User> CreateUser(Models.LocalDb.User user)
        {
            OnUserCreated(user);

            var existingItem = Context.Users
                              .Where(i => i.Id == user.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
                throw new Exception("Item already available");
            }

            try
            {
                Context.Users.Add(user);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(user).State = EntityState.Detached;
                throw;
            }

            OnAfterUserCreated(user);

            return user;
        }

        partial void OnAppointmentDeleted(Models.LocalDb.Appointment item);
        partial void OnAfterAppointmentDeleted(Models.LocalDb.Appointment item);

        public async Task<Models.LocalDb.Appointment> DeleteAppointment(Guid? id)
        {
            var itemToDelete = Context.Appointments
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
                throw new Exception("Item no longer available");
            }

            OnAppointmentDeleted(itemToDelete);

            Context.Appointments.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAppointmentDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnAppointmentGet(Models.LocalDb.Appointment item);

        public async Task<Models.LocalDb.Appointment> GetAppointmentById(Guid? id)
        {
            var items = Context.Appointments
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.User);

            items = items.Include(i => i.Person);

            var itemToReturn = items.FirstOrDefault();

            OnAppointmentGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.LocalDb.Appointment> CancelAppointmentChanges(Models.LocalDb.Appointment item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
                entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
                entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAppointmentUpdated(Models.LocalDb.Appointment item);
        partial void OnAfterAppointmentUpdated(Models.LocalDb.Appointment item);

        public async Task<Models.LocalDb.Appointment> UpdateAppointment(Guid? id, Models.LocalDb.Appointment appointment)
        {
            OnAppointmentUpdated(appointment);

            var itemToUpdate = Context.Appointments
                              .Where(i => i.Id == id)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
                throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(appointment);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();

            OnAfterAppointmentUpdated(appointment);

            return appointment;
        }

        partial void OnCosmeticServiceDeleted(Models.LocalDb.CosmeticService item);
        partial void OnAfterCosmeticServiceDeleted(Models.LocalDb.CosmeticService item);

        public async Task<Models.LocalDb.CosmeticService> DeleteCosmeticService(Guid? id)
        {
            var itemToDelete = Context.CosmeticServices
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
                throw new Exception("Item no longer available");
            }

            OnCosmeticServiceDeleted(itemToDelete);

            Context.CosmeticServices.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCosmeticServiceDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnCosmeticServiceGet(Models.LocalDb.CosmeticService item);

        public async Task<Models.LocalDb.CosmeticService> GetCosmeticServiceById(Guid? id)
        {
            var items = Context.CosmeticServices
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            var itemToReturn = items.FirstOrDefault();

            OnCosmeticServiceGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.LocalDb.CosmeticService> CancelCosmeticServiceChanges(Models.LocalDb.CosmeticService item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
                entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
                entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCosmeticServiceUpdated(Models.LocalDb.CosmeticService item);
        partial void OnAfterCosmeticServiceUpdated(Models.LocalDb.CosmeticService item);

        public async Task<Models.LocalDb.CosmeticService> UpdateCosmeticService(Guid? id, Models.LocalDb.CosmeticService cosmeticService)
        {
            OnCosmeticServiceUpdated(cosmeticService);

            var itemToUpdate = Context.CosmeticServices
                              .Where(i => i.Id == id)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
                throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(cosmeticService);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();

            OnAfterCosmeticServiceUpdated(cosmeticService);

            return cosmeticService;
        }

        partial void OnOrderDeleted(Models.LocalDb.Order item);
        partial void OnAfterOrderDeleted(Models.LocalDb.Order item);

        public async Task<Models.LocalDb.Order> DeleteOrder(Guid? id)
        {
            var itemToDelete = Context.Orders
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
                throw new Exception("Item no longer available");
            }

            OnOrderDeleted(itemToDelete);

            Context.Orders.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterOrderDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnOrderGet(Models.LocalDb.Order item);

        public async Task<Models.LocalDb.Order> GetOrderById(Guid? id)
        {
            var items = Context.Orders
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Person);

            items = items.Include(i => i.User);

            var itemToReturn = items.FirstOrDefault();

            OnOrderGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.LocalDb.Order> CancelOrderChanges(Models.LocalDb.Order item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
                entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
                entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnOrderUpdated(Models.LocalDb.Order item);
        partial void OnAfterOrderUpdated(Models.LocalDb.Order item);

        public async Task<Models.LocalDb.Order> UpdateOrder(Guid? id, Models.LocalDb.Order order)
        {
            OnOrderUpdated(order);

            var itemToUpdate = Context.Orders
                              .Where(i => i.Id == id)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
                throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(order);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();

            OnAfterOrderUpdated(order);

            return order;
        }

        partial void OnPersonDeleted(Models.LocalDb.Person item);
        partial void OnAfterPersonDeleted(Models.LocalDb.Person item);

        public async Task<Models.LocalDb.Person> DeletePerson(Guid? id)
        {
            var itemToDelete = Context.People
                              .Where(i => i.Id == id)
                              .Include(i => i.Appointments)
                              .Include(i => i.Orders)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
                throw new Exception("Item no longer available");
            }

            OnPersonDeleted(itemToDelete);

            Context.People.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterPersonDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnPersonGet(Models.LocalDb.Person item);

        public async Task<Models.LocalDb.Person> GetPersonById(Guid? id)
        {
            var items = Context.People
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            var itemToReturn = items.FirstOrDefault();

            OnPersonGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.LocalDb.Person> CancelPersonChanges(Models.LocalDb.Person item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
                entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
                entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnPersonUpdated(Models.LocalDb.Person item);
        partial void OnAfterPersonUpdated(Models.LocalDb.Person item);

        public async Task<Models.LocalDb.Person> UpdatePerson(Guid? id, Models.LocalDb.Person person)
        {
            OnPersonUpdated(person);

            var itemToUpdate = Context.People
                              .Where(i => i.Id == id)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
                throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(person);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();

            OnAfterPersonUpdated(person);

            return person;
        }

        partial void OnSubscriptionDeleted(Models.LocalDb.Subscription item);
        partial void OnAfterSubscriptionDeleted(Models.LocalDb.Subscription item);

        public async Task<Models.LocalDb.Subscription> DeleteSubscription(Guid? id)
        {
            var itemToDelete = Context.Subscriptions
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
                throw new Exception("Item no longer available");
            }

            OnSubscriptionDeleted(itemToDelete);

            Context.Subscriptions.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSubscriptionDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnSubscriptionGet(Models.LocalDb.Subscription item);

        public async Task<Models.LocalDb.Subscription> GetSubscriptionById(Guid? id)
        {
            var items = Context.Subscriptions
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            var itemToReturn = items.FirstOrDefault();

            OnSubscriptionGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.LocalDb.Subscription> CancelSubscriptionChanges(Models.LocalDb.Subscription item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
                entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
                entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSubscriptionUpdated(Models.LocalDb.Subscription item);
        partial void OnAfterSubscriptionUpdated(Models.LocalDb.Subscription item);

        public async Task<Models.LocalDb.Subscription> UpdateSubscription(Guid? id, Models.LocalDb.Subscription subscription)
        {
            OnSubscriptionUpdated(subscription);

            var itemToUpdate = Context.Subscriptions
                              .Where(i => i.Id == id)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
                throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(subscription);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();

            OnAfterSubscriptionUpdated(subscription);

            return subscription;
        }

        partial void OnUserDeleted(Models.LocalDb.User item);
        partial void OnAfterUserDeleted(Models.LocalDb.User item);

        public async Task<Models.LocalDb.User> DeleteUser(Guid? id)
        {
            var itemToDelete = Context.Users
                              .Where(i => i.Id == id)
                              .Include(i => i.Appointments)
                              .Include(i => i.Orders)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
                throw new Exception("Item no longer available");
            }

            OnUserDeleted(itemToDelete);

            Context.Users.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterUserDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnUserGet(Models.LocalDb.User item);

        public async Task<Models.LocalDb.User> GetUserById(Guid? id)
        {
            var items = Context.Users
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            var itemToReturn = items.FirstOrDefault();

            OnUserGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.LocalDb.User> CancelUserChanges(Models.LocalDb.User item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
                entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
                entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnUserUpdated(Models.LocalDb.User item);
        partial void OnAfterUserUpdated(Models.LocalDb.User item);

        public async Task<Models.LocalDb.User> UpdateUser(Guid? id, Models.LocalDb.User user)
        {
            OnUserUpdated(user);

            var itemToUpdate = Context.Users
                              .Where(i => i.Id == id)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
                throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(user);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();

            OnAfterUserUpdated(user);

            return user;
        }
    }
}

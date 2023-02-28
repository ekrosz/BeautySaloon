using BeautySaloon.Common.Exceptions;
using BeautySaloon.Core.Dto.Common;
using BeautySaloon.Core.Dto.Requests.Appointment;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Repositories.Abstract;
using BeautySaloon.DAL.Uow;

namespace BeautySaloon.Core.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IWriteRepository<Appointment> _appointmentWriteRepositry;

    private readonly IQueryRepository<Appointment> _appointmentQueryRepositry;

    private readonly IQueryRepository<PersonSubscription> _personSubscriptionQueryRepositry;

    private readonly IQueryRepository<Person> _personQueryRepositry;

    private readonly IUnitOfWork _unitOfWork;

    public AppointmentService(
        IWriteRepository<Appointment> appointmentWriteRepositry,
        IQueryRepository<Appointment> appointmentQueryRepositry,
        IQueryRepository<PersonSubscription> personSubscriptionQueryRepositry,
        IQueryRepository<Person> personQueryRepositry,
        IUnitOfWork unitOfWork)
    {
        _appointmentWriteRepositry = appointmentWriteRepositry;
        _appointmentQueryRepositry = appointmentQueryRepositry;
        _personSubscriptionQueryRepositry = personSubscriptionQueryRepositry;
        _personQueryRepositry = personQueryRepositry;
        _unitOfWork = unitOfWork;
    }

    public async Task CreateAppointmentAsync(CreateAppointmentRequestDto request, CancellationToken cancellationToken = default)
    {
        var person = await _personQueryRepositry.GetByIdAsync(request.PersonId, cancellationToken)
            ?? throw new EntityNotFoundException($"Клиент {request.PersonId} не найден.", typeof(Person));

        var personSubscriptions = person.Orders.SelectMany(
            x => x.PersonSubscriptions.Where(
                ps => request.PersonSubcriptionIds.Contains(ps.Id)));

        ValidateStatus(personSubscriptions);

        var duration = personSubscriptions.Sum(x => x.SubscriptionCosmeticService.CosmeticService.ExecuteTime);

        await ValidateDate(request.AppointmentDate, duration, null, cancellationToken);

        var entity = new Appointment(request.AppointmentDate, duration);

        entity.AddPersonSubscription(personSubscriptions);

        await _appointmentWriteRepositry.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAppointmentAsync(ByIdWithDataRequestDto<UpdateAppointmentRequestDto> request, CancellationToken cancellationToken = default)
    {
        var entity = await _appointmentWriteRepositry.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException($"Запись {request.Id} не найдена.", typeof(Appointment));

        var personSubscriptions = await _personSubscriptionQueryRepositry.FindAsync(
            x => request.Data.PersonSubcriptionIds.Contains(x.Id),
            cancellationToken);

        ValidateStatus(personSubscriptions);

        var duration = personSubscriptions.Sum(x => x.SubscriptionCosmeticService.CosmeticService.ExecuteTime);

        await ValidateDate(request.Data.AppointmentDate, duration, request.Id, cancellationToken);

        entity.AddPersonSubscription(personSubscriptions);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static void ValidateStatus(IEnumerable<PersonSubscription> personSubscriptions)
    {
        foreach (var personSubscription in personSubscriptions)
        { 
            if (personSubscription.Status == PersonSubscriptionStatus.NotPaid)
            {
                throw new Exception("Абонемент еще не оплачен");
            }

            if (personSubscription.Status == PersonSubscriptionStatus.Cancelled)
            {
                throw new Exception("Абонемент был отменен");
            }

            if (personSubscription.Status == PersonSubscriptionStatus.Completed || personSubscription.AppointmentId.HasValue)
            {
                throw new Exception("Уже создана запись на данную услугу");
            }
        }
    }

    public async Task CompleteAppointmentAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        var entity = await _appointmentWriteRepositry.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException($"Запись {request.Id} не найдена.", typeof(Appointment));

        entity.Complete();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task CancelAppointmentAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        var entity = await _appointmentWriteRepositry.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException($"Запись {request.Id} не найдена.", typeof(Appointment));

        entity.Cancel();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task ValidateDate(
        DateTime appointmentDate,
        int duration,
        Guid? appointmentId,
        CancellationToken cancellationToken = default)
    {
        var endDateTine = appointmentDate.AddMinutes(duration);

        var anyAppointmentInRange = await _appointmentQueryRepositry.ExistAsync(
            x => (!appointmentId.HasValue || appointmentId.Value != x.Id)
            && (x.AppointmentDate < endDateTine && x.AppointmentDate > appointmentDate)
                || (x.AppointmentDate.AddMinutes(x.DurationInMinutes) < endDateTine && x.AppointmentDate.AddMinutes(x.DurationInMinutes) > appointmentDate),
            cancellationToken);

        if (anyAppointmentInRange)
        {
            throw new Exception($"В период времени с {appointmentDate:G} до {appointmentDate:G} уже существует запись.");
        }
    }
}

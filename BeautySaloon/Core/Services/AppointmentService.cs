using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.Appointment;
using BeautySaloon.Api.Dto.Responses.Appointment;
using AutoMapper;
using BeautySaloon.Common.Exceptions;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using BeautySaloon.DAL.Repositories.Abstract;
using BeautySaloon.DAL.Uow;

namespace BeautySaloon.Core.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IWriteRepository<Appointment> _appointmentWriteRepositry;

    private readonly IQueryRepository<Appointment> _appointmentQueryRepositry;

    private readonly IQueryRepository<PersonSubscription> _personSubscriptionQueryRepositry;

    private readonly IWriteRepository<Person> _personWriteRepositry;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;

    public AppointmentService(
        IWriteRepository<Appointment> appointmentWriteRepositry,
        IQueryRepository<Appointment> appointmentQueryRepositry,
        IQueryRepository<PersonSubscription> personSubscriptionQueryRepositry,
        IWriteRepository<Person> personWriteRepositry,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _appointmentWriteRepositry = appointmentWriteRepositry;
        _appointmentQueryRepositry = appointmentQueryRepositry;
        _personSubscriptionQueryRepositry = personSubscriptionQueryRepositry;
        _personWriteRepositry = personWriteRepositry;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task CreateAppointmentAsync(CreateAppointmentRequestDto request, CancellationToken cancellationToken = default)
    {
        var person = await _personWriteRepositry.GetByIdAsync(request.PersonId, cancellationToken)
            ?? throw new PersonNotFoundException(request.PersonId);

        var personSubscriptions = person.Orders.SelectMany(
            x => x.PersonSubscriptions.Where(
                ps => request.PersonSubcriptionIds.Contains(ps.Id)));

        var notExistPersonSubscription = request.PersonSubcriptionIds
            .Except(personSubscriptions.Select(x => x.Id))
            .ToArray();

        if (notExistPersonSubscription.Any())
        {
            throw new PersonSubscriptionNotFoundException(notExistPersonSubscription.First());
        }

        ValidateStatus(personSubscriptions);

        var duration = personSubscriptions.Sum(x => x.SubscriptionCosmeticServiceSnapshot.CosmeticServiceSnapshot.ExecuteTimeInMinutes ?? 0);

        await ValidateDate(
            personSubscriptions,
            request.AppointmentDate,
            duration,
            null,
            cancellationToken);

        var entity = new Appointment(request.AppointmentDate, duration, request.Comment);

        entity.AddPersonSubscription(personSubscriptions);
        person.AddAppointment(entity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAppointmentAsync(ByIdWithDataRequestDto<UpdateAppointmentRequestDto> request, CancellationToken cancellationToken = default)
    {
        var entity = await _appointmentWriteRepositry.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new AppointmentNotFoundException(request.Id);

        var personSubscriptions = await _personSubscriptionQueryRepositry.FindAsync(
            x => request.Data.PersonSubcriptionIds.Contains(x.Id),
            cancellationToken);

        var notExistPersonSubscription = request.Data.PersonSubcriptionIds
            .Except(personSubscriptions.Select(x => x.Id))
            .ToArray();

        if (notExistPersonSubscription.Any())
        {
            throw new PersonSubscriptionNotFoundException(notExistPersonSubscription.First());
        }

        ValidateStatus(personSubscriptions);

        var duration = personSubscriptions.Sum(x => x.SubscriptionCosmeticServiceSnapshot.CosmeticServiceSnapshot.ExecuteTimeInMinutes ?? 0);

        await ValidateDate(
            personSubscriptions,
            request.Data.AppointmentDate,
            duration,
            request.Id,
            cancellationToken);

        entity.Update(request.Data.AppointmentDate, duration, request.Data.Comment);
        entity.AddPersonSubscription(personSubscriptions);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task CompleteAppointmentAsync(ByIdWithDataRequestDto<CompleteOrCancelAppointmentRequestDto> request, CancellationToken cancellationToken = default)
    {
        var entity = await _appointmentWriteRepositry.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new AppointmentNotFoundException(request.Id);

        entity.Complete(request.Data.Comment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task CancelAppointmentAsync(ByIdWithDataRequestDto<CompleteOrCancelAppointmentRequestDto> request, CancellationToken cancellationToken = default)
    {
        var entity = await _appointmentWriteRepositry.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new AppointmentNotFoundException(request.Id);

        entity.Cancel(request.Data.Comment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<PageResponseDto<GetAppointmentListItemResponseDto>> GetAppointmentListAsync(GetAppointmentListRequestDto request, CancellationToken cancellationToken = default)
    {
        var appointments = await _appointmentQueryRepositry.GetPageAsync(
            request: request.Page,
            predicate: x => (string.IsNullOrWhiteSpace(request.SearchString)
                    || string.Join(' ', x.Person.Name.LastName, x.Person.Name.FirstName, x.Person.Name.MiddleName).TrimEnd(' ').ToLower().Contains(request.SearchString.ToLower())
                        || x.Person.PhoneNumber.ToLower().Contains(request.SearchString.ToLower()))
                    && ((!request.StartAppointmentDate.HasValue || x.AppointmentDate.Date >= request.StartAppointmentDate.Value.Date)
                        && (!request.EndAppointmentDate.HasValue || x.AppointmentDate.Date <= request.EndAppointmentDate.Value.Date)),
            sortProperty: x => x.AppointmentDate,
            asc: false,
            cancellationToken);

        return _mapper.Map<PageResponseDto<GetAppointmentListItemResponseDto>>(appointments);
    }

    public async Task<GetAppointmentResponseDto> GetAppointmentAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        var appointment = await _appointmentQueryRepositry.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new AppointmentNotFoundException(request.Id);

        return _mapper.Map<GetAppointmentResponseDto>(appointment);
    }

    private static void ValidateStatus(IEnumerable<PersonSubscription> personSubscriptions)
    {
        foreach (var personSubscription in personSubscriptions)
        {
            personSubscription.ValidateStatusOrThrow();
        }
    }

    private async Task ValidateDate(
        IEnumerable<PersonSubscription> personSubscriptions,
        DateTime appointmentDate,
        int duration,
        Guid? appointmentId,
        CancellationToken cancellationToken = default)
    {
        var endDateTime = appointmentDate.AddMinutes(duration);

        var anyAppointmentInRange = await _appointmentQueryRepositry.ExistAsync(
            x => (!appointmentId.HasValue || appointmentId.Value != x.Id)
            && (x.AppointmentDate < endDateTime && x.AppointmentDate > appointmentDate)
                || (x.AppointmentDate.AddMinutes(x.DurationInMinutes) < endDateTime && x.AppointmentDate.AddMinutes(x.DurationInMinutes) > appointmentDate),
            cancellationToken);

        if (anyAppointmentInRange)
        {
            throw new InvalidAppointmentDateException(appointmentDate, endDateTime);
        }

        foreach (var personSubscription in personSubscriptions)
        {
            var subscriptionLifeTime = personSubscription.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.LifeTimeInDays;

            if (subscriptionLifeTime.HasValue && personSubscription.Order.UpdatedOn.AddDays(subscriptionLifeTime.Value).Date < appointmentDate.Date)
            {
                throw new PersonSubscriptionWillBeOverdueException(
                    personSubscription.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Name,
                    personSubscription.SubscriptionCosmeticServiceSnapshot.CosmeticServiceSnapshot.Name);
            }
        }
    }
}

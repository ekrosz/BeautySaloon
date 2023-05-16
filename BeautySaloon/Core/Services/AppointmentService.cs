using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.Appointment;
using BeautySaloon.Api.Dto.Responses.Appointment;
using AutoMapper;
using BeautySaloon.Common.Exceptions;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Repositories.Abstract;
using BeautySaloon.DAL.Uow;
using BeautySaloon.DAL.Entities.Enums;

namespace BeautySaloon.Core.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IWriteRepository<Appointment> _appointmentWriteRepositry;

    private readonly IWriteRepository<PersonSubscription> _personSubscriptionWriteRepositry;

    private readonly IWriteRepository<Person> _personWriteRepositry;

    private readonly IQueryRepository<Appointment> _appointmentQueryRepositry;

    private readonly IQueryRepository<Order> _orderQueryRepositry;


    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;

    public AppointmentService(
        IWriteRepository<Appointment> appointmentWriteRepositry,
        IWriteRepository<PersonSubscription> personSubscriptionWriteRepositry,
        IWriteRepository<Person> personWriteRepositry,
        IQueryRepository<Appointment> appointmentQueryRepositry,
        IQueryRepository<Order> orderQueryRepositry,
        IQueryRepository<Person> personQueryRepositry,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _appointmentWriteRepositry = appointmentWriteRepositry;
        _personSubscriptionWriteRepositry = personSubscriptionWriteRepositry;
        _personWriteRepositry = personWriteRepositry;
        _appointmentQueryRepositry = appointmentQueryRepositry;
        _orderQueryRepositry = orderQueryRepositry;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task CreateAppointmentAsync(CreateAppointmentRequestDto request, CancellationToken cancellationToken = default)
    {
        var person = await _personWriteRepositry.GetByIdAsync(request.PersonId, cancellationToken)
            ?? throw new PersonNotFoundException(request.PersonId);

        var personSubscriptions = person.Orders.SelectMany(
            x => x.PersonSubscriptions.Where(
                ps => request.PersonSubscriptionIds.Contains(ps.Id)));

        var notExistPersonSubscription = request.PersonSubscriptionIds
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

        var personSubscriptions = await _personSubscriptionWriteRepositry.FindAsync(
            x => request.Data.PersonSubscriptionIds.Contains(x.Id),
            cancellationToken);

        var notExistPersonSubscription = request.Data.PersonSubscriptionIds
            .Except(personSubscriptions.Select(x => x.Id))
            .ToArray();

        if (notExistPersonSubscription.Any())
        {
            throw new PersonSubscriptionNotFoundException(notExistPersonSubscription.First());
        }

        ValidateStatus(personSubscriptions.Where(x => !entity.PersonSubscriptions.Select(y => y.Id).Contains(x.Id)));

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

    public async Task<ItemListResponseDto<GetAppointmentListItemResponseDto>> GetAppointmentListAsync(GetAppointmentListRequestDto request, CancellationToken cancellationToken = default)
    {
        var appointments = await _appointmentQueryRepositry.FindAsync(
            predicate: x => (string.IsNullOrWhiteSpace(request.SearchString)
                    || string.Join(' ', x.Person.Name.LastName, x.Person.Name.FirstName, x.Person.Name.MiddleName).TrimEnd(' ').ToLower().Contains(request.SearchString.ToLower())
                        || x.Person.PhoneNumber.ToLower().Contains(request.SearchString.ToLower()))
                    && x.PersonSubscriptions.Any(),
            cancellationToken: cancellationToken);

        return new ItemListResponseDto<GetAppointmentListItemResponseDto>(_mapper.Map<IReadOnlyCollection<GetAppointmentListItemResponseDto>>(appointments));
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
            && x.PersonSubscriptions.Any()
            && x.PersonSubscriptions.All(y => y.Status == PersonSubscriptionCosmeticServiceStatus.InProgress)
            && ((x.AppointmentDate < endDateTime && x.AppointmentDate > appointmentDate)
                || (x.AppointmentDate.AddMinutes(x.DurationInMinutes) < endDateTime && x.AppointmentDate.AddMinutes(x.DurationInMinutes) > appointmentDate)),
            cancellationToken);

        if (anyAppointmentInRange)
        {
            throw new InvalidAppointmentDateException(appointmentDate, endDateTime);
        }

        foreach (var personSubscription in personSubscriptions)
        {
            var subscriptionLifeTime = personSubscription.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.LifeTimeInDays;

            var order = await _orderQueryRepositry.GetByIdAsync(personSubscription.OrderId, cancellationToken)
                ?? throw new OrderNotFoundException(personSubscription.OrderId);

            if (subscriptionLifeTime.HasValue && order.UpdatedOn.AddDays(subscriptionLifeTime.Value).Date < appointmentDate.Date)
            {
                throw new PersonSubscriptionWillBeOverdueException(
                    personSubscription.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Name,
                    personSubscription.SubscriptionCosmeticServiceSnapshot.CosmeticServiceSnapshot.Name);
            }
        }
    }
}

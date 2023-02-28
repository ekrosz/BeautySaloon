using AutoMapper;
using BeautySaloon.Common.Exceptions;
using BeautySaloon.Core.Dto.Common;
using BeautySaloon.Core.Dto.Requests.Person;
using BeautySaloon.Core.Dto.Responses.Person;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using BeautySaloon.DAL.Repositories.Abstract;
using BeautySaloon.DAL.Uow;

namespace BeautySaloon.Core.Services;

public class PersonService : IPersonService
{
    private readonly IQueryRepository<Person> _personQueryRepository;

    private readonly IWriteRepository<Person> _personWriteRepository;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;

    public PersonService(
        IQueryRepository<Person> personQueryRepository,
        IWriteRepository<Person> personWriteRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _personQueryRepository = personQueryRepository;
        _personWriteRepository = personWriteRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task CreatePersonAsync(CreatePersonRequestDto request, CancellationToken cancellationToken = default)
    {
        var entity = new Person(
            request.Name,
            request.BirthDate,
            request.PhoneNumber,
            request.Email,
            request.Comment);

        await _personWriteRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeletePersonAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        var person = await _personWriteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException($"Пользователь {request.Id} не найден.", typeof(Person));

        _personWriteRepository.Delete(person);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetPersonResponseDto> GetPersonAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        var person = await _personQueryRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException($"Пользователь {request.Id} не найден.", typeof(Person));

        return _mapper.Map<GetPersonResponseDto>(person);
    }

    public async Task<PageResponseDto<GetPersonListItemResponseDto>> GetPersonListAsync(GetPersonListRequestDto request, CancellationToken cancellationToken = default)
    {
        var page = await _personQueryRepository.GetPageAsync(
            request: request.Page,
            predicate: x => string.IsNullOrWhiteSpace(request.SearchString)
                || string.Join(' ', x.Name.LastName, x.Name.FirstName, x.Name.MiddleName).TrimEnd(' ').ToLower().Contains(request.SearchString.ToLower())
                || x.PhoneNumber.ToLower().Contains(request.SearchString.ToLower()),
            sortProperty: x => x.Name.LastName,
            cancellationToken: cancellationToken);

        return _mapper.Map<PageResponseDto<GetPersonListItemResponseDto>>(page);
    }

    public async Task UpdatePersonAsync(ByIdWithDataRequestDto<UpdatePersonRequestDto> request, CancellationToken cancellationToken = default)
    {
        var person = await _personWriteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException($"Пользователь {request.Id} не найден.", typeof(Person));

        person.Update(
            request.Data.Name,
            request.Data.BirthDate,
            request.Data.PhoneNumber,
            request.Data.Email,
            request.Data.Comment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

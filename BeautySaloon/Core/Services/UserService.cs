using AutoMapper;
using BeautySaloon.Core.Dto.Common;
using BeautySaloon.Core.Dto.Requests.User;
using BeautySaloon.Core.Dto.Responses.User;
using BeautySaloon.Core.Exceptions;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Repositories.Abstract;
using BeautySaloon.DAL.Uow;

namespace BeautySaloon.Core.Services;

public class UserService : IUserService
{
    private readonly IQueryRepository<User> _userQueryRepository;

    private readonly IWriteRepository<User> _userWriteRepository;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;

    public UserService(
        IQueryRepository<User> userQueryRepository,
        IWriteRepository<User> userWriteRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _userQueryRepository = userQueryRepository;
        _userWriteRepository = userWriteRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task CreateUserAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _userWriteRepository.GetFirstAsync(x => x.Login.Equals(request.Login), cancellationToken);

        if (user is not null)
        {
            throw new EntityAlreadyExistException($"Пользователь {user.Login} уже существуюет", typeof(User));
        }

        var entity = new User(
            request.Role,
            request.Login,
            request.Password,
            request.Name,
            request.PhoneNumber,
            request.Email);

        await _userWriteRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUserAsync(ByIdWithDataRequestDto<UpdateUserRequestDto> request, CancellationToken cancellationToken = default)
    {
        var user = await _userWriteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException($"Пользователь {request.Id} не найден.", typeof(User));

        var isExistLogin = await _userWriteRepository.ExistAsync(x => x.Login.Equals(request.Data.Login) && x.Id != request.Id, cancellationToken);

        if (isExistLogin)
        {
            throw new EntityAlreadyExistException($"Пользователь {request.Data.Login} уже существуюет", typeof(User));
        }

        user.Update(
            request.Data.Role,
            request.Data.Login,
            request.Data.Password,
            request.Data.Name,
            request.Data.PhoneNumber,
            request.Data.Email);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _userWriteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException($"Пользователь {request.Id} не найден.", typeof(User));

        _userWriteRepository.Delete(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetUserResponseDto> GetUserAsync(ByIdRequestDto request, CancellationToken cancellationToken)
    {
        var user = await _userQueryRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException($"Пользователь {request.Id} не найден.", typeof(User));

        return _mapper.Map<GetUserResponseDto>(user);
    }

    public async Task<ItemListResponseDto<GetUserResponseDto>> GetUserListAsync(GetUserListRequestDto request, CancellationToken cancellationToken = default)
    {
        var users = await _userQueryRepository.FindAsync(x =>
            string.IsNullOrWhiteSpace(request.SearchString)
            || string.Join(' ', x.Name.LastName, x.Name.FirstName, x.Name.MiddleName).TrimEnd(' ').ToLower().Contains(request.SearchString.ToLower())

            || x.PhoneNumber.ToLower().Contains(request.SearchString.ToLower()), cancellationToken);

        return new ItemListResponseDto<GetUserResponseDto>(_mapper.Map<IReadOnlyCollection<GetUserResponseDto>>(users));
    }
}

﻿using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.User;
using BeautySaloon.Api.Dto.Responses.User;
using AutoMapper;
using BeautySaloon.Common.Exceptions;
using BeautySaloon.Common.Utils;
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
        var isExistLogin = await _userQueryRepository.ExistAsync(
            x => x.Login.Equals(request.Login),
            cancellationToken);

        if (isExistLogin)
        {
            throw new UserAlreadyExistException(nameof(request.Login), request.Login);
        }

        var normalizedPhoneNumber = PhoneNumberUtilities.Normilize(request.PhoneNumber);

        var isExistPhone = await _userQueryRepository.ExistAsync(
            x => x.PhoneNumber.ToLower().Equals(normalizedPhoneNumber.ToLower()),
            cancellationToken);

        if (isExistPhone)
        {
            throw new PersonAlreadyExistException(nameof(request.PhoneNumber), request.PhoneNumber);
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
                ?? throw new UserNotFoundException(request.Id);

        var isExistLogin = await _userQueryRepository.ExistAsync(
            x => x.Login.Equals(request.Data.Login)
                && x.Id != request.Id,
            cancellationToken);

        if (isExistLogin)
        {
            throw new UserAlreadyExistException(nameof(user.Login), user.Login);
        }

        var normalizedPhoneNumber = PhoneNumberUtilities.Normilize(request.Data.PhoneNumber);

        var isExistPhone = await _userQueryRepository.ExistAsync(
            x => x.Id != request.Id
                && x.PhoneNumber.ToLower().Equals(normalizedPhoneNumber.ToLower()),
            cancellationToken);

        if (isExistPhone)
        {
            throw new PersonAlreadyExistException(nameof(request.Data.PhoneNumber), request.Data.PhoneNumber);
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
            ?? throw new UserNotFoundException(request.Id);

        _userWriteRepository.Delete(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetUserResponseDto> GetUserAsync(ByIdRequestDto request, CancellationToken cancellationToken)
    {
        var user = await _userQueryRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new UserNotFoundException(request.Id);

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

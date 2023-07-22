using AutoMapper;
using BCrypt;
using BudgetApp.Core.Common;
using BudgetApp.Core.Features.Auth.Models;
using BudgetApp.Domain.Repositories.Interfaces;
using MediatR;

namespace BudgetApp.Core.Features.Auth.Commands;

public class LoginUserCommand : IRequest<ExecutionResult<LoginResultModel>>
{
    public LoginModel User { get; init; }
}

public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ExecutionResult<LoginResultModel>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public LoginUserCommandHandler(IUserRepository userRepository, IMediator mediator,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<ExecutionResult<LoginResultModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var userEntity = await _userRepository.GetByEmail(request.User.Email);
        if (userEntity is null)
        {
            return new ExecutionResult<LoginResultModel>(new ErrorInfo(ErrorCode.LoginError, MessageCode.InvalidEmailOrPassword));
        }

        if (!BCryptHelper.CheckPassword(request.User.Password, userEntity.PasswordHash))
        {
            return new ExecutionResult<LoginResultModel>(new ErrorInfo(ErrorCode.LoginError, MessageCode.InvalidEmailOrPassword));
        }

        var token = await _mediator.Send(new SignUserTokenCommand
        {
            UserEntity = userEntity
        }, cancellationToken);
        
        return new ExecutionResult<LoginResultModel>(new LoginResultModel()
        {
            Token = token,
            User = 
        });
    }
}
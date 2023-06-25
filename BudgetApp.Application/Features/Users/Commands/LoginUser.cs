using BCrypt;
using BudgetApp.Core.Common;
using BudgetApp.Core.Features.Users.Models;
using BudgetApp.Domain.Repositories.Interfaces;
using MediatR;

namespace BudgetApp.Core.Features.Users.Commands;

public class LoginUserCommand : IRequest<ExecutionResult<LoginResultModel>>
{
    public LoginModel User { get; init; }
}

public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ExecutionResult<LoginResultModel>>
{
    private readonly IUserRepository userRepository;
    private readonly IMediator mediator;

    public LoginUserCommandHandler(IUserRepository userRepository, IMediator mediator)
    {
        this.userRepository = userRepository;
        this.mediator = mediator;
    }

    public async Task<ExecutionResult<LoginResultModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var userEntity = await userRepository.GetByEmail(request.User.Email);
        if (userEntity is null)
        {
            return new ExecutionResult<LoginResultModel>(new ErrorInfo(ErrorCode.LoginError, MessageCode.InvalidEmailOrPassword));
        }

        if (!BCryptHelper.CheckPassword(request.User.Password, userEntity.PasswordHash))
        {
            return new ExecutionResult<LoginResultModel>(new ErrorInfo(ErrorCode.LoginError, MessageCode.InvalidEmailOrPassword));
        }

        var token = await mediator.Send(new SignUserTokenCommand
        {
            UserEntity = userEntity
        }, cancellationToken);
        
        return new ExecutionResult<LoginResultModel>(new LoginResultModel()
        {
            Token = token
        });
    }
}
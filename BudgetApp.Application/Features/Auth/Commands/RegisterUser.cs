using BCrypt;
using BudgetApp.Core.Features.Auth.Models;
using BudgetApp.Core.Services;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Repositories.Interfaces;
using MediatR;

namespace BudgetApp.Core.Features.Auth.Commands;

public class RegisterUserCommand : IRequest
{
    public LoginModel User { get; set; }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IMediator _mediator;

    public RegisterUserCommandHandler(IUserRepository userRepository, IMediator mediator)
    {
        _userRepository = userRepository;
        _mediator = mediator;
    }

    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var salt = BCryptHelper.GenerateSalt();
        var userEntity = new UserEntity
        {
            Email = request.User.Email,
            PasswordHash = BCryptHelper.HashPassword(request.User.Password, salt),
            CreateDate = TimeService.Now,
            UpdateDate = TimeService.Now
        };

        var createUser = await _userRepository.CreateAsync(userEntity);

        await _mediator.Send(new SignUserTokenCommand
        {
            UserEntity = createUser
        }, cancellationToken);
    }
}
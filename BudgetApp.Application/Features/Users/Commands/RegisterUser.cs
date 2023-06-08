using BCrypt;
using BudgetApp.Core.Features.Users.Models;
using BudgetApp.Core.Services;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Repositories.Interfaces;
using MediatR;

namespace BudgetApp.Core.Features.Users.Commands;

public class RegisterUserCommand : IRequest
{
    public User User { get; set; }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
{
    private readonly IUserRepository userRepository;
    private readonly IMediator mediator;

    public RegisterUserCommandHandler(IUserRepository userRepository, IMediator mediator)
    {
        this.userRepository = userRepository;
        this.mediator = mediator;
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

        var createUser = await userRepository.CreateAsync(userEntity);

        await mediator.Send(new SignUserTokenCommand
        {
            UserEntity = createUser
        }, cancellationToken);
    }
}
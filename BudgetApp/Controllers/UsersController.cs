using BudgetApp.Core.Features.Users.Commands;
using BudgetApp.Core.Features.Users.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers;

public class UsersController : ApiControllerBase
{
    private readonly IMediator mediator;

    public UsersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<LoginResultModel> Login([FromBody] LoginModel model)
    {
        var login = await mediator.Send(new LoginUserCommand()
        {
            User = model
        });

        return login.Value;
    }
}
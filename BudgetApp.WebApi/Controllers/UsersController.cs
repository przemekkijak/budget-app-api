using BudgetApp.Core.Features.Auth.Commands;
using BudgetApp.Core.Features.Auth.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers;

[Controller]
[Route("api/users")]
public class UsersController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<LoginResultModel> Login([FromBody] LoginModel model)
    {
        var login = await _mediator.Send(new LoginUserCommand()
        {
            User = model
        });

        return login.Value;
    }
}
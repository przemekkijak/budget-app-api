using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace BudgetApp.Core.Features.Auth.Commands;

public class LogoutUserCommand : IRequest
{
    
}

public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand>
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public LogoutUserCommandHandler(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }
    
    public async Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        await httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
using System.Security.Claims;
using BudgetApp.Core.Common;
using BudgetApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace BudgetApp.Core.Features.Users.Commands;

public class SignUserTokenCommand : IRequest
{
    public UserEntity UserEntity { get; init; }
}

public class SignUserTokenCommandHandler : IRequestHandler<SignUserTokenCommand>
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public SignUserTokenCommandHandler(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(SignUserTokenCommand request, CancellationToken cancellationToken)
    {
        var userClaims = new List<Claim>
        {
            new(ClaimTypes.Email, request.UserEntity.Email),
            new(CustomClaimTypes.Id, request.UserEntity.Id.ToString()),
        };
        
        var claimsIdentity = new ClaimsIdentity(
            userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties();
        await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity), authProperties);
    }
}
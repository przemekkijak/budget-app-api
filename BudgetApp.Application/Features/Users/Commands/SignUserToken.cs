using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BudgetApp.Core.Common;
using BudgetApp.Domain;
using BudgetApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace BudgetApp.Core.Features.Users.Commands;

public class SignUserTokenCommand : IRequest<string>
{
    public UserEntity UserEntity { get; init; }
}

public class SignUserTokenCommandHandler : IRequestHandler<SignUserTokenCommand, string>
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly AppSettings appSettings;

    public SignUserTokenCommandHandler(IHttpContextAccessor httpContextAccessor, AppSettings appSettings)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.appSettings = appSettings;
    }

    public async Task<string> Handle(SignUserTokenCommand request, CancellationToken cancellationToken)
    {
        var userClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, request.UserEntity.Email),
            new Claim(CustomClaimTypes.Id, request.UserEntity.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.TokenSecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: appSettings.TokenIssuer,
            audience: appSettings.TokenSigningKey,
            claims: userClaims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

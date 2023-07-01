using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BudgetApp.Core.Common;
using BudgetApp.Core.Services;
using BudgetApp.Domain;
using BudgetApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace BudgetApp.Core.Features.Auth.Commands;

public class SignUserTokenCommand : IRequest<string>
{
    public UserEntity UserEntity { get; init; }
}

public class SignUserTokenCommandHandler : IRequestHandler<SignUserTokenCommand, string>
{
    private readonly AppSettings appSettings;

    public SignUserTokenCommandHandler(IHttpContextAccessor httpContextAccessor, AppSettings appSettings)
    {
        this.appSettings = appSettings;
    }

    public async Task<string> Handle(SignUserTokenCommand request, CancellationToken cancellationToken)
    {
        var userClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, request.UserEntity.Email),
            new Claim(CustomClaimTypes.Id, request.UserEntity.Id.ToString())
        };
        
        var tokenLifetime = TimeSpan.FromDays(7);
        
        var jwt = new JwtSecurityToken(
            issuer: appSettings.TokenIssuer,
            audience: appSettings.TokenIssuer,
            notBefore: TimeService.Now,
            claims: userClaims,
            expires: TimeService.Now.Add(tokenLifetime),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.TokenSigningKey)),
                SecurityAlgorithms.HmacSha256Signature));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}

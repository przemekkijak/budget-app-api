using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BudgetApp.Domain;
using BudgetApp.Models;
using Microsoft.IdentityModel.Tokens;

namespace BudgetApp.Core;

public class UserService : ServiceBase
{
    private readonly AppSettings _appSettings;

    public UserService(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    private string CreateToken(UserModel user)
    {
        var now = DateTime.Now;
        var userClaims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new("Id", user.Id.ToString()),
        };
     
        var tokenLifetime = TimeSpan.FromDays(1);
        var jwt = new JwtSecurityToken(
            issuer: _appSettings.TokenIssuer,
            audience: _appSettings.TokenIssuer,
            notBefore: now,
            claims: userClaims,
            expires: now.Add(tokenLifetime),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.TokenSigningKey)),
                SecurityAlgorithms.HmacSha256Signature));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt;
using BudgetApp.Domain;
using BudgetApp.Domain.Common;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Interfaces.Repositories;
using BudgetApp.Domain.Interfaces.Services;
using Lisek.Domain;
using Microsoft.IdentityModel.Tokens;

namespace BudgetApp.Core;

public class UserService : IUserService
{
    private readonly AppSettings _appSettings;
    private readonly IUserRepository _userRepository;

    public UserService(AppSettings appSettings, IUserRepository userRepository)
    {
        _appSettings = appSettings;
        _userRepository = userRepository;
    }

    public async Task<ExecutionResult<UserEntity>> GetProfile(int userId)
    {
        var userEntity = await _userRepository.GetByIdAsync(userId);
        return new ExecutionResult<UserEntity>(userEntity);
    }

    public async Task<ExecutionResult<LoginResult>> AuthenticateUser(User model)
    {
        var userEntity = await _userRepository.GetByEmail(model.Email);
        if (userEntity is null)
        {
            return new ExecutionResult<LoginResult>(new ErrorInfo(ErrorCode.LoginError, MessageCode.InvalidEmailOrPassword));
        }

        if (!BCryptHelper.CheckPassword(model.Password, userEntity.PasswordHash))
        {
            return new ExecutionResult<LoginResult>(new ErrorInfo(ErrorCode.LoginError, MessageCode.InvalidEmailOrPassword));
        }

        var token = CreateToken(userEntity);
        return new ExecutionResult<LoginResult>()
        {
            Value = new LoginResult()
            {
                Token = token,
                User = userEntity
            }
        };
    }
    
    public async Task<LoginResult?> RegisterUser(User model)
    {
        var salt = BCryptHelper.GenerateSalt();
        var userEntity = new UserEntity()
        {
            Email = model.Email,
            PasswordHash = BCryptHelper.HashPassword(model.Password, salt),
            CreateDate = TimeService.Now,
            UpdateDate = TimeService.Now
        };

        var createUser = await _userRepository.CreateAsync(userEntity);
        var token = CreateToken(createUser);
        return new LoginResult()
        {
            Token = token
        };
    }

    private string CreateToken(UserEntity user)
    {
        var now = DateTime.Now;
        var userClaims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(CustomClaimTypes.Id, user.Id.ToString()),
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
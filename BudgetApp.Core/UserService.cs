using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt;
using BudgetApp.Core.Interfaces.Services;
using BudgetApp.Domain;
using BudgetApp.Domain.Common;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Interfaces.Repositories;
using BudgetApp.Domain.Models;
using BudgetApp.Domain.Objects;
using Microsoft.IdentityModel.Tokens;

namespace BudgetApp.Core;

public class UserService : IUserService
{
    private readonly AppSettings appSettings;
    private readonly IUserRepository userRepository;

    public UserService(AppSettings appSettings, IUserRepository userRepository)
    {
        this.appSettings = appSettings;
        this.userRepository = userRepository;
    }

    public async Task<ExecutionResult<UserModel>> GetProfile(int userId)
    {
        var userEntity = await userRepository.GetByIdAsync(userId);
        if (userEntity is null)
        {
            return new ExecutionResult<UserModel>(new ErrorInfo(ErrorCode.UserNotFound, MessageCode.UserNotFound));
        }

        return new ExecutionResult<UserModel>(ModelFactory.Create(userEntity));
    }

    public async Task<ExecutionResult<LoginResultModel>> AuthenticateUser(User model)
    {
        var userEntity = await userRepository.GetByEmail(model.Email);
        if (userEntity is null)
        {
            return new ExecutionResult<LoginResultModel>(new ErrorInfo(ErrorCode.LoginError, MessageCode.InvalidEmailOrPassword));
        }

        if (!BCryptHelper.CheckPassword(model.Password, userEntity.PasswordHash))
        {
            return new ExecutionResult<LoginResultModel>(new ErrorInfo(ErrorCode.LoginError, MessageCode.InvalidEmailOrPassword));
        }

        var token = CreateToken(userEntity);
        return new ExecutionResult<LoginResultModel>()
        {
            Value = new LoginResultModel()
            {
                Token = token,
                User = ModelFactory.Create(userEntity)
            }
        };
    }
    
    public async Task<ExecutionResult<LoginResultModel>> RegisterUser(User model)
    {
        var salt = BCryptHelper.GenerateSalt();
        var userEntity = new UserEntity()
        {
            Email = model.Email,
            PasswordHash = BCryptHelper.HashPassword(model.Password, salt),
            CreateDate = TimeService.Now,
            UpdateDate = TimeService.Now
        };

        var createUser = await userRepository.CreateAsync(userEntity);
        var token = CreateToken(createUser);
        return new ExecutionResult<LoginResultModel>()
        {
            Value = new LoginResultModel()
            {
                Token = token,
                User = ModelFactory.Create(userEntity) 
            }
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
            issuer: appSettings.TokenIssuer,
            audience: appSettings.TokenIssuer,
            notBefore: now,
            claims: userClaims,
            expires: now.Add(tokenLifetime),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.TokenSigningKey)),
                SecurityAlgorithms.HmacSha256Signature));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
    
    //TODO RefreshToken
}
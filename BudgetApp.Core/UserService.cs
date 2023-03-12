using System.Security.Claims;
using BCrypt;
using BudgetApp.Core.Interfaces.Services;
using BudgetApp.Domain;
using BudgetApp.Domain.Common;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Interfaces.Repositories;
using BudgetApp.Domain.Models;
using BudgetApp.Domain.Objects;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace BudgetApp.Core;

public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    private readonly IHttpContextAccessor httpContextAccessor;

    public UserService(IUserRepository userRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        this.userRepository = userRepository;
        this.httpContextAccessor = httpContextAccessor;
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

        await AuthenticateUser(userEntity);
        return new ExecutionResult<LoginResultModel>();
    }

    public async Task Logout()
    {
        await httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
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
        await AuthenticateUser(createUser);
        return new ExecutionResult<LoginResultModel>() { };
    }

    private async Task AuthenticateUser(UserEntity user)
    {
        var now = DateTime.Now;
        var userClaims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(CustomClaimTypes.Id, user.Id.ToString()),
        };
        
        var claimsIdentity = new ClaimsIdentity(
            userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties();
        await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity), authProperties);
    }
    
    //TODO RefreshToken
}
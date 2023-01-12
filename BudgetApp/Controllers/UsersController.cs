using BudgetApp.Core.Interfaces.Services;
using BudgetApp.Domain;
using BudgetApp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers;

[Route("api/users")]
public class UsersController : ApiControllerBase
{
    private readonly IUserService userService;

    public UsersController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpGet]
    [Route("test")]
    public async Task<ExecutionResult> Test()
    {
        return new ExecutionResult();
    }

    [HttpPost]
    [Route("register")]
    public async Task<ExecutionResult<LoginResultModel>> Register([FromBody] RegistrationModel model)
    {
        var user = ModelFactory.Create(model);
        return await userService.RegisterUser(user);
    }

    [HttpPost]
    [Route("login")]
    public async Task<ExecutionResult<LoginResultModel>> Login([FromBody] RegistrationModel model)
    {
        var user = ModelFactory.Create(model);
        return await userService.AuthenticateUser(user);
    }

    [Authorize]
    [HttpGet]
    public async Task<ExecutionResult<UserModel>> GetProfile()
    {
       return await userService.GetProfile(UserId);
    }
}
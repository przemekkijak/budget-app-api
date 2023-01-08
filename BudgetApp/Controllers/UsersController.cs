using BudgetApp.Core;
using BudgetApp.Models;
using Lisek.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers;

[Route("users")]
public class UsersController : ApiControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<ExecutionResult<LoginResultModel?>> Register([FromBody] RegistrationModel model)
    {
        var user = ModelFactory.Create(model);
        
        var register = await _userService.RegisterUser(user);
        
        LoginResultModel? resultModel = null;
        
        if (register != null)
            resultModel = ModelFactory.Create(register);
        
        return new ExecutionResult<LoginResultModel?>(resultModel);
    }

    [HttpPost]
    [Route("login")]
    public async Task<ExecutionResult<LoginResultModel>> Login([FromBody] RegistrationModel model)
    {
        var user = ModelFactory.Create(model);
        
        var authenticateUser = await _userService.AuthenticateUser(user);

        return new ExecutionResult<LoginResultModel>()
        {
            Errors = authenticateUser.Errors,
            Success = authenticateUser.Success,
            Value = ModelFactory.Create(authenticateUser?.Value)
        };
    }

    [Authorize]
    [HttpGet]
    public async Task<ExecutionResult<UserModel>> GetProfile()
    {
        var user = await _userService.GetProfile(UserId);
        return new ExecutionResult<UserModel>()
        {
            Errors = user.Errors,
            Value = ModelFactory.Create(user.Value)
        };
    }
}
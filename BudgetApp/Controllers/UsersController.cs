using BudgetApp.Core;
using BudgetApp.Domain;
using BudgetApp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers;

[Route("api/users")]
public class UsersController : ApiControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<ExecutionResult<LoginResultModel>> Register([FromBody] RegistrationModel model)
    {
        var user = ModelFactory.Create(model);
        return await _userService.RegisterUser(user);
    }

    [HttpPost]
    [Route("login")]
    public async Task<ExecutionResult<LoginResultModel>> Login([FromBody] RegistrationModel model)
    {
        var user = ModelFactory.Create(model);
        return await _userService.AuthenticateUser(user);
    }

    [Authorize]
    [HttpGet]
    public async Task<ExecutionResult<UserModel>> GetProfile()
    {
       return await _userService.GetProfile(UserId);
    }
}
using BudgetApp.Data;
using BudgetApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers;

[Route("test")]
public class TestController : Controller
{
    private readonly UserData _userData;

    public TestController(UserData userData)
    {
        _userData = userData;
    }

    [HttpGet]
    public async Task<UserEntity> Test()
    {
        return await _userData.GetUserById(1);
    }
}
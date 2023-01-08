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
    [Route("{id}")]
    public async Task<UserEntity> Test(int id)
    {
        return await _userData.GetByIdAsync(id);
    }

    [HttpPost]
    public async Task<UserEntity> Create([FromBody] UserEntity user)
    {
        return await _userData.CreateAsync(user);
    }
}
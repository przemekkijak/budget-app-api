using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers;

[Route("users")]
public class UsersController : Controller
{
    public UsersController()
    {
        
    }
    
    [HttpPost]
    [Route("login")]
    public async Task
}
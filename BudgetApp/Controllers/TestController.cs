using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers;

[Route("test")]
public class TestController : Controller
{
    public TestController()
    {
        
    }

    [HttpGet]
    public string Test()
    {
        return "test";
    }
}
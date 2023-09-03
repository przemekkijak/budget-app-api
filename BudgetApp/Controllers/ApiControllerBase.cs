using BudgetApp.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers;

[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected int UserId => User.GetUserId();
}
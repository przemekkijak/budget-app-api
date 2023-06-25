using BudgetApp.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    protected int UserId => User.GetUserId();
}
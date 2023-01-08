using BudgetApp.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers;

public class ApiControllerBase : ControllerBase
{
    protected int UserId => User.GetUserId();

}
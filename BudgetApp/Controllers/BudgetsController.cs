using BudgetApp.Core.Interfaces.Services;
using BudgetApp.Domain;
using BudgetApp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers;

[Route("api/budgets")]
[Authorize]
public class BudgetsController : ApiControllerBase
{
    private readonly IBudgetService budgetService;

    public BudgetsController(IBudgetService budgetService)
    {
        this.budgetService = budgetService;
    }

    [HttpGet]
    public async Task<ExecutionResult<BudgetModel>> GetDefault()
    {
        return await budgetService.GetDefault(UserId);
    }

    [HttpPost]
    public async Task<ExecutionResult<BudgetModel>> CreateBudget([FromBody] BudgetModel model)
    {
        return await budgetService.CreateBudget(UserId, model);
    }
}
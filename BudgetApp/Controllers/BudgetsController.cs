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
    private readonly IBudgetService _budgetService;

    public BudgetsController(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }

    [HttpGet]
    public async Task<ExecutionResult<BudgetModel>> GetDefault()
    {
        return await _budgetService.GetDefault(UserId);
    }

    [HttpPost]
    public async Task<ExecutionResult<BudgetModel>> CreateBudget([FromBody] BudgetModel model)
    {
        return await _budgetService.CreateBudget(UserId, model);
    }
}
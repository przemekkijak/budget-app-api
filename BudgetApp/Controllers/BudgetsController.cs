using BudgetApp.Core.Interfaces.Services;
using BudgetApp.Domain;
using BudgetApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BudgetApp.Controllers;

[Route("api/budgets")]
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
}
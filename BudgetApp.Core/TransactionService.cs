using BudgetApp.Core.Interfaces.Services;
using BudgetApp.Domain;
using BudgetApp.Domain.Common;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.Domain.Interfaces.Repositories;
using BudgetApp.Domain.Models;

namespace BudgetApp.Core;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository transactionRepository;
    private readonly IBudgetRepository budgetRepository;
    private readonly IBankAccountService bankAccountService;

    public TransactionService(ITransactionRepository transactionRepository, 
        IBudgetRepository budgetRepository,
        IBankAccountService bankAccountService)
    {
        this.transactionRepository = transactionRepository;
        this.budgetRepository = budgetRepository;
        this.bankAccountService = bankAccountService;
    }

    public async Task<List<TransactionModel>> GetForBudget(int budgetId, bool currentMonthOnly = false)
    {
        DateTime? startDate = null;
        DateTime? endDate = null;

        if (currentMonthOnly)
        {
            var now = TimeService.Now;
            startDate = new DateTime(now.Year, now.Month, 1);
            endDate = new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1);
        }

        var entities = await transactionRepository.GetForBudget(budgetId, startDate, endDate);
        return entities.Select(ModelFactory.Create).ToList();
    }

    public async Task<ExecutionResult> AddTransaction(int userId, TransactionModel model)
    {
        var budget = await budgetRepository.GetByIdAsync(model.BudgetId);
        if (budget is null)
        {
            return new ExecutionResult(new ErrorInfo(ErrorCode.BudgetError, MessageCode.BudgetNotFound));
        }

        var canPerformAction =
            await IsUserAuthorizedToPerformActionOnBudget(userId, budget, TransactionActionEnum.Write);

        if (!canPerformAction)
        {
            return new ExecutionResult(new ErrorInfo(ErrorCode.BudgetError, MessageCode.Unauthorized));
        }

        var transactionEntity = new TransactionEntity
        {
            BudgetId = model.BudgetId,
            Amount = model.Amount,
            UserId = userId,
            Status = model.Status,
            Description = model.Description,
            CreateDate = TimeService.Now,
            UpdateDate = TimeService.Now,
            IsDeleted = false,
            BankAccountId = model.BankAccountId
        };

        await transactionRepository.CreateAsync(transactionEntity);
        if (transactionEntity.Status == TransactionStatusEnum.Completed)
        {
            await bankAccountService.UpdateAccountAmount(transactionEntity.BankAccountId, transactionEntity.Amount);
        }
        
        return new ExecutionResult();
    }

    public async Task<ExecutionResult> UpdateTransaction(int userId, TransactionModel model)
    {
        var transaction = await transactionRepository.GetByIdAsync(model.Id);
        if (transaction is null)
        {
            return await AddTransaction(userId, model);
        }

        var budget = await budgetRepository.GetByIdAsync(transaction.BudgetId);
        if (budget is null)
        {
            //TODO Log critical - create logging service 
            return new ExecutionResult<bool>(new ErrorInfo(ErrorCode.BudgetError, MessageCode.BudgetNotFound));
        }
        
        var canPerformAction =
            await IsUserAuthorizedToPerformActionOnBudget(userId, budget, TransactionActionEnum.Write);

        if (!canPerformAction)
        {
            return new ExecutionResult<bool>(new ErrorInfo(ErrorCode.BudgetError, MessageCode.Unauthorized));
        }

        transaction.Amount = model.Amount;
        transaction.Status = model.Status;
        transaction.BankAccountId = model.BankAccountId;
        transaction.UpdateDate = TimeService.Now;

        
        var update = await transactionRepository.UpdateAsync(transaction);
        
        if (transaction.Status == TransactionStatusEnum.Completed)
        {
            await bankAccountService.UpdateAccountAmount(transaction.BankAccountId, transaction.Amount);
        }
        //TODO what if false? Handle and log error, those two operations should be done in transaction (for later) 
        
        return new ExecutionResult<bool>(update);
    }

    public async Task<ExecutionResult> DeleteTransaction(int userId, int transactionId)
    {
        var transaction = await transactionRepository.GetByIdAsync(transactionId);
        if (transaction is null)
        {
            return new ExecutionResult<bool>(new ErrorInfo(ErrorCode.TransactionError, MessageCode.TransactionNotFound));
        }
        
        var budget = await budgetRepository.GetByIdAsync(transaction.BudgetId);
        if (budget is null)
        {
            //TODO Log critical - create logging service 
            return new ExecutionResult<bool>(new ErrorInfo(ErrorCode.BudgetError, MessageCode.BudgetNotFound));
        }
        
        var canPerformAction =
            await IsUserAuthorizedToPerformActionOnBudget(userId, budget, TransactionActionEnum.Write);
        
        if (!canPerformAction)
        {
            return new ExecutionResult<bool>(new ErrorInfo(ErrorCode.BudgetError, MessageCode.Unauthorized));
        }

        transaction.IsDeleted = true;
        transaction.UpdateDate = TimeService.Now;
        await transactionRepository.UpdateAsync(transaction);
        
        return new ExecutionResult();
    }

    private async Task<bool> IsUserAuthorizedToPerformActionOnBudget(int userId, BudgetEntity budget,
        TransactionActionEnum action)
    {
        if (userId == budget.UserId)
            return true;

        return false;
        //TODO check budget permissions 
    }
}
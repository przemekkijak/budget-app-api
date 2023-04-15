using BudgetApp.Core;
using BudgetApp.Domain.Common;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Interfaces.Repositories;
using BudgetApp.Domain.Models;
using NSubstitute;

namespace BudgetApp.Tests;

public class TransactionServiceTests
{
    [Fact]
    public async Task ShouldNotAddTransactionUserWithoutWritePermission()
    {
        //Arrange
        var transactionRepository = Substitute.For<ITransactionRepository>();
        var budgetRepository = Substitute.For<IBudgetRepository>();
        var transactionService = new TransactionService(transactionRepository, budgetRepository);

        var budget = new BudgetEntity
        {
            Id = 1,
            UserId = 2
        };

        var user = new UserEntity
        {
            Id = 1
        };

        var transaction = new TransactionModel
        {
            Amount = 5,
            BudgetId = 1,
        };

        budgetRepository.GetByIdAsync(budget.Id).Returns(budget);
        
        //Act
        var result = await transactionService.AddTransaction(user.Id, transaction);

        //Assert
        Assert.False(result.Success);
        Assert.Contains(result.Errors, a => a.MessageCode == MessageCode.Unauthorized);
    }
}
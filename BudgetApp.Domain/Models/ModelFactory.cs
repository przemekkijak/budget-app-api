using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Objects;

namespace BudgetApp.Domain.Models;

public class ModelFactory
{
    public static LoginResultModel Create(LoginResult? loginResult)
    {
        if (loginResult is null)
            return new LoginResultModel();
        
        return new LoginResultModel()
        {
            Token = loginResult.Token,
            RefreshToken = loginResult.RefreshToken,
            User = Create(loginResult.User)
        };
    }

    public static UserModel Create(UserEntity? entity)
    {
        if (entity is null)
            return new UserModel();
        
        return new UserModel()
        {
            Id = entity.Id,
            Email = entity.Email,
            CreateDate = entity.CreateDate,
            UpdateDate = entity.UpdateDate
        };
    }

    public static User Create(RegistrationModel model)
    {
        return new User()
        {
            Email = model.Email,
            Password = model.Password
        };
    }

    public static BudgetModel Create(BudgetEntity entity)
    {
        return new BudgetModel()
        {
            Id = entity.Id,
            Name = entity.Name,
            UserId = entity.UserId,
            IsDefault = entity.IsDefault
        };
    }

    public static TransactionModel Create(TransactionEntity entity)
    {
        return new TransactionModel()
        {
            BudgetId = entity.BudgetId,
            Amount = entity.Amount,
            Status = entity.Status,
            UserId = entity.UserId,
            Description = entity.Description,
            CreateDate = entity.CreateDate,
            UpdateDate = entity.UpdateDate
        };
    }
}
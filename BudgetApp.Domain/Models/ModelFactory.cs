﻿using BudgetApp.Domain.Entities;
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

    public static UserModel Create(UserEntity? user)
    {
        if (user is null)
            return new UserModel();
        
        return new UserModel()
        {
            Id = user.Id,
            Email = user.Email,
            CreateDate = user.CreateDate,
            UpdateDate = user.UpdateDate
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

    public static BudgetModel Create(BudgetEntity budget)
    {
        return new BudgetModel()
        {
            Id = budget.Id,
            Name = budget.Name,
            UserId = budget.UserId,
            IsDefault = budget.IsDefault
        };
    }
}
﻿using BudgetApp.Domain;
using BudgetApp.Domain.Models;
using BudgetApp.Domain.Objects;

namespace BudgetApp.Core.Interfaces.Services;

public interface IUserService
{
    Task<ExecutionResult<LoginResultModel>> RegisterUser(User model);
    Task<ExecutionResult<LoginResultModel>> AuthenticateUser(User model);
    Task<ExecutionResult<UserModel>> GetProfile(int userId);

}
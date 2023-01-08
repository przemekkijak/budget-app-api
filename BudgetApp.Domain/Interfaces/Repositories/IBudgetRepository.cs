﻿
using BudgetApp.Domain.Entities;

namespace BudgetApp.Domain.Interfaces.Repositories;

public interface IBudgetRepository : IBaseRepository<BudgetEntity>
{
    Task<BudgetEntity?> GetDefault(int userId);
}
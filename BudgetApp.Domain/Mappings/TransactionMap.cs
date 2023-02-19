using BudgetApp.Domain.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace BudgetApp.Domain.Mappings;

public class TransactionMap : DommelEntityMap<TransactionEntity>
{
    public TransactionMap()
    {
        ToTable("transactions");

        Map(x => x.Id).ToColumn("id").IsKey();
        Map(x => x.BudgetId).ToColumn("budget_id");
        Map(x => x.Amount).ToColumn("amount");
        Map(x => x.Status).ToColumn("status");
        Map(x => x.CreateDate).ToColumn("create_date");
        Map(x => x.UpdateDate).ToColumn("update_date");
        Map(x => x.Description).ToColumn("description");
    }
}
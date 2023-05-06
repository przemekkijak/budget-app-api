using BudgetApp.Domain.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace BudgetApp.Domain.EntityMappings;

public class BankAccountMap : DommelEntityMap<BankAccountEntity>
{
    public BankAccountMap()
    {
        ToTable("bank_accounts");

        Map(x => x.Id).ToColumn("id").IsKey();
        Map(x => x.Name).ToColumn("name");
        Map(x => x.UserId).ToColumn("user_id");
        Map(x => x.BudgetId).ToColumn("budget_id");
        Map(x => x.NumberSuffix).ToColumn("number_suffix");
        Map(x => x.CreateDate).ToColumn("create_date");
        Map(x => x.UpdateDate).ToColumn("update_date");
        Map(x => x.Amount).ToColumn("amount");
        Map(x => x.IsDefault).ToColumn("is_default");
    }
}
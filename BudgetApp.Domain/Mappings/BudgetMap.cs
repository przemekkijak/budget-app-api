using BudgetApp.Domain.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace BudgetApp.Domain.Mappings;

public class BudgetMap : DommelEntityMap<BudgetEntity>
{
    public BudgetMap()
    {
        ToTable("budgets");

        Map(x => x.UserId).ToColumn("user_id");
        Map(x => x.Name).ToColumn("name");
        Map(x => x.IsDefault).ToColumn("is_default");
    }
}
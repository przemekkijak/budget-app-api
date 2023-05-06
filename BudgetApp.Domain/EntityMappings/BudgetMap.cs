using BudgetApp.Domain.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace BudgetApp.Domain.EntityMappings;

public class BudgetMap : DommelEntityMap<BudgetEntity>
{
    public BudgetMap()
    {
        ToTable("budgets");

        Map(x => x.Id).ToColumn("id").IsKey();
        Map(x => x.UserId).ToColumn("user_id");
        Map(x => x.Name).ToColumn("name");
        Map(x => x.IsDefault).ToColumn("is_default");
        Map(x => x.CreateDate).ToColumn("create_date");
        Map(x => x.UpdateDate).ToColumn("update_date");
    }
}
using BudgetApp.Domain.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace BudgetApp.Domain.EntityMappings;

public class EntityBaseMap : DommelEntityMap<EntityBase>
{
    public EntityBaseMap()
    {
        Map(x => x.Id).ToColumn("id");
        Map(x => x.CreateDate).ToColumn("create_date");
        Map(x => x.UpdateDate).ToColumn("update_date");
    }
}
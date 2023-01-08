using BudgetApp.Domain.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace BudgetApp.Domain.Mappings;

public class EntityBaseMap : DommelEntityMap<EntityBase>
{
    public EntityBaseMap()
    {
        Map(x => x.Id).ToColumn("id");
    }
}
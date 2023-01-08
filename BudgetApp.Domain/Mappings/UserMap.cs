using BudgetApp.Domain.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace BudgetApp.Domain.Mappings;

public class UserMap : DommelEntityMap<UserEntity>
{
    public UserMap()
    {
        ToTable("users");
        
        Map(x => x.Id).ToColumn("id").IsKey();
        Map(x => x.Email).ToColumn("email");
        Map(x => x.PasswordHash).ToColumn("password_hash");
        Map(x => x.CreateDate).ToColumn("create_date");
        Map(x => x.UpdateDate).ToColumn("update_date");
    }
}
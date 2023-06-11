using BudgetApp.Domain.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace BudgetApp.Domain.EntityMappings;

public class ImportTransactionSchemeMap : DommelEntityMap<ImportTransactionSchemeEntity>
{
    public ImportTransactionSchemeMap()
    {
        ToTable("import_transaction_schemes");

        Map(x => x.Id).ToColumn("id").IsKey();
        Map(x => x.UserId).ToColumn("user_id");
        Map(x => x.Name).ToColumn("name");
        Map(x => x.PaymentDateIndex).ToColumn("payment_date_index");
        Map(x => x.DescriptionIndex).ToColumn("description_index");
        Map(x => x.AmountIndex).ToColumn("amount_index");
        Map(x => x.BankAccountId).ToColumn("bank_account_id");
    }
}
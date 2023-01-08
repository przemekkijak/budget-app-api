namespace BudgetApp.Domain;

public class AppSettings
{
    public string ConnectionString { get; set; }

    public string TokenIssuer { get; set; }

    public string TokenSigningKey { get; set; }
}
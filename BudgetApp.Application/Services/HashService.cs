using System.Security.Cryptography;
using System.Text;

namespace BudgetApp.Core.Services;

public static class HashService
{
    public static string GetSha256Hash(string input)
    {
        using var hash = SHA256.Create();
        var bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        var builder = new StringBuilder();
            
        foreach (var t in bytes)
        {
            builder.Append(t.ToString("x2"));
        }

        return builder.ToString();
    }
}
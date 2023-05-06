using System.Security.Claims;
using BudgetApp.Core.Common;

namespace BudgetApp.Infrastructure;

public static class CLaimsHelper
{
    private const string ZeroValue = "0";

    public static string GetClaim(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        return claimsPrincipal.Claims.FirstOrDefault(x => x.Type == claimType)?.Value;
    }

    public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        if (int.TryParse(claimsPrincipal.GetClaim(CustomClaimTypes.Id) ?? ZeroValue, out int accountId))
        {
            return accountId;
        }

        return 0;
    }
}
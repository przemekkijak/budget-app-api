using System.Security.Claims;
using BudgetApp.Domain.Common;
using Microsoft.AspNetCore.Components.Authorization;

namespace BudgetApp.Helpers;

public static class ClaimsHelper
{
    public static int GetUserId(this AuthenticationState context)
    {
        var idClaimValue = context.User.Claims.FirstOrDefault(a => a.Type == CustomClaimTypes.Id)?.Value;
        if (string.IsNullOrWhiteSpace(idClaimValue))
            return 0;

        return int.Parse(idClaimValue);
    }
    
    public static int GetUserId(this HttpContext context)
    {
        var idClaimValue = context.User.Claims.FirstOrDefault(a => a.Type == CustomClaimTypes.Id)?.Value;
        if (string.IsNullOrWhiteSpace(idClaimValue))
            return 0;

        return int.Parse(idClaimValue);
    }

    public static string GetUserEmail(this AuthenticationState context)
    {
        return context.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Email)?.Value ?? string.Empty;
    }
}
using System.Collections.Concurrent;
using BudgetApp.Core.Interfaces.Services;
using BudgetApp.Domain.Objects;

namespace BudgetApp.Middlewares;

public class BlazorCookieLoginMiddleware
{
    public static IDictionary<Guid, User> Logins { get; }
        = new ConcurrentDictionary<Guid, User>();        


    private readonly RequestDelegate _next;

    public BlazorCookieLoginMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService userService)
    {
        if (context.Request.Path == "/login" && context.Request.Query.ContainsKey("key"))
        {
            var key = Guid.Parse(context.Request.Query["key"]);
            var info = Logins[key];

            await userService.AuthenticateUser(info);
            context.Response.Redirect("/");
            await _next.Invoke(context);
        }
        else if (context.Request.Path == "/logout")
        {
            await userService.Logout();
            context.Response.Redirect("/");
            await _next.Invoke(context);
        }
        else
        {
            await _next.Invoke(context);
        }
    }
}
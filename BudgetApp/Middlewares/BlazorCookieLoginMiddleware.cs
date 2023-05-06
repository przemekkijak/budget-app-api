using System.Collections.Concurrent;
using BudgetApp.Core.Features.Users.Commands;
using BudgetApp.Core.Features.Users.Models;
using MediatR;

namespace BudgetApp.Middlewares;

public class BlazorCookieLoginMiddleware
{
    public static IDictionary<Guid, User> Logins { get; } = new ConcurrentDictionary<Guid, User>();        
    
    private readonly RequestDelegate next;

    public BlazorCookieLoginMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context, IMediator mediator)
    {
        if (context.Request.Path == "/login" && context.Request.Query.ContainsKey("key"))
        {
            var key = Guid.Parse(context.Request.Query["key"]);
            var info = Logins[key];

            await mediator.Send(new LoginUserCommand()
            {
                User = info
            });
            
            context.Response.Redirect("/");
            await next.Invoke(context);
        }
        else if (context.Request.Path == "/logout")
        {
            await mediator.Send(new LogoutUserCommand());
            
            context.Response.Redirect("/");
            await next.Invoke(context);
        }
        else
        {
            await next.Invoke(context);
        }
    }
}
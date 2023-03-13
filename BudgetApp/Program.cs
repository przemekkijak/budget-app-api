using BudgetApp.Core;
using BudgetApp.Core.Interfaces.Services;
using BudgetApp.Domain;
using BudgetApp.Domain.Interfaces.Repositories;
using BudgetApp.Domain.Mappings;
using BudgetApp.Domain.Repositories;
using BudgetApp.Middlewares;
using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

ConfigureDapper();
AddServices();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<BlazorCookieLoginMiddleware>();


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

void AddServices()
{
    //Blazor
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();

    //AppSettings
    var appSettingsSection = builder.Configuration.GetSection("AppSettings");
    builder.Services.Configure<AppSettings>(appSettingsSection);
    var appSettings = appSettingsSection.Get<AppSettings>();
    builder.Services.AddSingleton(appSettings);
    
    //Auth
    ConfigureAuth(appSettings);
    builder.Services.AddAuthorization();

    //Base services
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Description = "Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new List<string>()
            }
        });
    });

    //Services and repositories
    builder.Services.AddScoped<DialogService>();
    builder.Services.AddScoped<NotificationService>();
    builder.Services.AddScoped<TooltipService>();
    builder.Services.AddScoped<ContextMenuService>();
    
    builder.Services.AddScoped<IBaseRepository, BaseRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
    builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<ITransactionService, TransactionService>();
    builder.Services.AddScoped<IBudgetService, BudgetService>();


}

void ConfigureDapper()
{
    DefaultTypeMap.MatchNamesWithUnderscores = true;
    FluentMapper.Initialize(c =>
    {
        c.AddMap(new EntityBaseMap());
        c.AddMap(new UserMap());
        c.AddMap(new BudgetMap());
        c.AddMap(new TransactionMap());
        c.ForDommel();
    });
 
}

void ConfigureAuth(AppSettings appSettings)
{
    builder.Services
        .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie();
}
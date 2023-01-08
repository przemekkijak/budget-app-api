using System.Text;
using BudgetApp.Core;
using BudgetApp.Data;
using BudgetApp.Domain;
using BudgetApp.Domain.Interfaces;
using BudgetApp.Domain.Interfaces.Repositories;
using BudgetApp.Domain.Mappings;
using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

ConfigureDapper();
AddServices();


var app = builder.Build();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.Run();


void AddServices()
{
    
    //AppSettings
    var appSettingsSection = builder.Configuration.GetSection("AppSettings");
    builder.Services.Configure<AppSettings>(appSettingsSection);
    var appSettings = appSettingsSection.Get<AppSettings>();
    builder.Services.AddSingleton(appSettings);
    
    //Auth
    ConfigureAuth(appSettings);
    builder.Services.AddAuthorization();

    //Base services
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
    builder.Services.AddScoped<IBaseRepository, BaseRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();

    builder.Services.AddScoped<UserService>();
}

void ConfigureDapper()
{
    DefaultTypeMap.MatchNamesWithUnderscores = true;
    FluentMapper.Initialize(c =>
    {
        c.AddMap(new EntityBaseMap());
        c.AddMap(new UserMap());
        c.ForDommel();
    });
 
}

void ConfigureAuth(AppSettings appSettings)
{
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = appSettings.TokenIssuer,

                ValidateAudience = true,
                ValidAudience = appSettings.TokenIssuer,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.TokenSigningKey))
            };
        });
}
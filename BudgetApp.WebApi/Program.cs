using System.Text;
using AutoMapper;
using BudgetApp.Core.Features.Auth.Models;
using BudgetApp.Core.Features.BankAccounts.Models;
using BudgetApp.Core.Features.Budgets.Models;
using BudgetApp.Core.Features.ImportTransactions.Models;
using BudgetApp.Core.Features.Transactions.Models;
using BudgetApp.Domain;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.EntityMappings;
using BudgetApp.Domain.Repositories;
using BudgetApp.Domain.Repositories.Interfaces;
using BudgetApp.Infrastructure.DapperUtcDate;
using BudgetApp.Infrastructure.Repositories;
using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureDapper();
AddServices();

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };
    });
    
    
    
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsOptionsBuilder =>
    {
        corsOptionsBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

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
    builder.Services.AddAuthorization(_ => { });

    //Base services
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
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
    
    //AutoMapper
    var mapper = PrepareAutoMapperConfig().CreateMapper();
    builder.Services.AddSingleton(mapper);

    //Services and repositories
    builder.Services.AddScoped<IBaseRepository, BaseRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
    builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
    builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
    builder.Services.AddScoped<IImportTransactionSchemeRepository, ImportTransactionSchemeRepository>();
    

    builder.Services.AddTransient<IMediator, Mediator>();
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
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
        c.AddMap(new BankAccountMap());
        c.AddMap(new ImportTransactionSchemeMap());
        c.ForDommel();
    });
 
    SqlMapper.AddTypeHandler(new DapperUtcDateTypeInterceptor());
}

void ConfigureAuth(AppSettings appSettings)
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = appSettings.TokenIssuer,

                ValidateAudience = true,
                ValidAudience = appSettings.TokenIssuer,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.TokenSigningKey)),

                ValidateLifetime = true,
                RequireExpirationTime = true,
            };
        });
}

MapperConfiguration PrepareAutoMapperConfig()
{
    //TODO improve creating maps, should be some internal method to create both ways
    
    return new MapperConfiguration(cfg => {
        cfg.CreateMap<TransactionEntity, TransactionModel>();
        cfg.CreateMap<UserEntity, UserModel>();
        cfg.CreateMap<BudgetEntity, BudgetModel>();
        cfg.CreateMap<BankAccountEntity, BankAccountModel>();
        
        cfg.CreateMap<ImportTransactionSchemeEntity, ImportTransactionSchemeModel>();
        cfg.CreateMap<ImportTransactionSchemeModel, ImportTransactionSchemeEntity>();
    });
}
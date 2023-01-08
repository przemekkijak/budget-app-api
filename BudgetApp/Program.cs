using BudgetApp.Data;
using BudgetApp.Domain;
using BudgetApp.Domain.Mappings;
using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;

var builder = WebApplication.CreateBuilder(args);

AddServices();

DefaultTypeMap.MatchNamesWithUnderscores = true;
FluentMapper.Initialize(c =>
{
    c.AddMap(new UserMap());
    c.ForDommel();
});


var app = builder.Build();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

void AddServices()
{
    var appSettingsSection = builder.Configuration.GetSection("AppSettings");
    builder.Services.Configure<AppSettings>(appSettingsSection);
    var appSettings = appSettingsSection.Get<AppSettings>();
    
    builder.Services.AddSingleton(appSettings);
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddTransient<UserData>();
}
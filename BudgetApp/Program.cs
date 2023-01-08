
using BudgetApp.Data;
using BudgetApp.Domain;

var builder = WebApplication.CreateBuilder(args);

AddServices();
var app = builder.Build();

app.MapControllers();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

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
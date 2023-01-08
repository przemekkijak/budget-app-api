var builder = WebApplication.CreateBuilder(args);

AddServices();
var app = builder.Build();

app.MapControllers();
app.Run();


void AddServices()
{
    builder.Services.AddControllers();
}
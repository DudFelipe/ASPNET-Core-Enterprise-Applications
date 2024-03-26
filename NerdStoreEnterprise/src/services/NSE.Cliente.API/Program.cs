using NSE.Cliente.API.Configuration;
using NSE.Cliente.API.Services;
using NSE.WebApi.Core.Identidade;

var builder = WebApplication.CreateBuilder(args);

#region Configurando para que a aplicação execute em ambientes distintos, utilizando appsettings distintos para cada ambiente

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

if (builder.Environment.Equals("Development"))
{
    builder.Configuration.AddUserSecrets<Program>();
}

#endregion

builder.Services.AddApiConfiguration(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddSwaggerConfiguration();
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.RegisterServices();
builder.Services.AddMessageBusConfiguration(builder.Configuration);

var app = builder.Build();

var service = app.Services.GetService<RegistroClienteIntegrationHandler>();
service.SetResponder();

app.UseSwaggerConfiguration();
app.UseApiConfiguration(app.Environment);

app.MapControllers();

app.Run();

using NSE.Identidade.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

#region Configurando para que a aplicação execute em ambientes distintos, utilizando appsettings distintos para cada ambiente

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

if(builder.Environment.Equals("Development"))
{
    builder.Configuration.AddUserSecrets<Program>();
}

#endregion

builder.Services.AddIdentityConfiguration(builder.Configuration);
builder.Services.AddApiConfiguration();
builder.Services.AddSwaggerConfiguration();


var app = builder.Build();

app.UseSwaggerConfiguration();
app.UseApiConfiguration(app.Environment);

app.MapControllers();

app.Run();

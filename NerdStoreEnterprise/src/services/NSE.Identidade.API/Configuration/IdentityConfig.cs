using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSE.Identidade.API.Data;
using NSE.Identidade.API.Extensions;
using System.Text;

namespace NSE.Identidade.API.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services,
                                                                  IConfiguration configuration)
        {
            //Configurando contexto de banco de dados para o Identity
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            //Configurando o Identity
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>() //Adicionando suporte para Roles de usuário
                .AddEntityFrameworkStores<ApplicationDbContext>() //Utilizando o Entity Framework para armazenar os dados do Identity no contexto ApplicationDbContext
                .AddDefaultTokenProviders() //Tokens para confirmação de email, reset de senha, etc
                .AddErrorDescriber<IdentityMensagensPortugues>(); //Utilizando mensagens em português no identity

            #region JWT

            var appSettingsSection = configuration.GetSection("AppSettings");//Obtendo a seção AppSettings do arquivo appsettings.json
            services.Configure<AppSettings>(appSettingsSection); //Informando que a seção AppSettings será mapeada para a classe AppSettings

            var appSettings = appSettingsSection.Get<AppSettings>(); //Criando uma instancia da classe AppSettings
            var key = Encoding.ASCII.GetBytes(appSettings.Secret); //Obtendo a chave de criptografia

            //Configurando o schema de autenticação
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = true;
                bearerOptions.SaveToken = true;
                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appSettings.ValidoEm,
                    ValidIssuer = appSettings.Emissor
                };
            });

            #endregion

            return services;
        }

        public static IApplicationBuilder UseIdentityConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}

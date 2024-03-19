using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NSE.Identidade.API.Data;
using NSE.Identidade.API.Extensions;
using NSE.WebApi.Core.Identidade;

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

            services.AddJwtConfiguration(configuration);

            return services;
        }
    }
}

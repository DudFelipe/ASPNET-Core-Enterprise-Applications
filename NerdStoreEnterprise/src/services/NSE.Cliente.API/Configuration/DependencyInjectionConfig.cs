using FluentValidation.Results;
using MediatR;
using NSE.Cliente.API.Application.Commands;
using NSE.Cliente.API.Application.Events;
using NSE.Cliente.API.Data;
using NSE.Cliente.API.Data.Repositories;
using NSE.Cliente.API.Models;
using NSE.Core.Mediator;
using NSE.WebApi.Core.Usuario;

namespace NSE.Cliente.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();
            services.AddScoped<IRequestHandler<AdicionarEnderecoCommand, ValidationResult>, ClienteCommandHandler>();
            services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClientEventHandler>();

            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<ClientesContext>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();
        }
    }
}

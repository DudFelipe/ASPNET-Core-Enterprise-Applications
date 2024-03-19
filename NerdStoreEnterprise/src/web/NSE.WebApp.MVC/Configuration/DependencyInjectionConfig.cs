using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Services;
using NSE.WebApp.MVC.Services.Handlers;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace NSE.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            //Registrando o serviço de autenticação como um HttpClient
            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();

            services.AddHttpClient<ICatalogoService, CatalogoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>() //Adicionando o delegating handler para manipular o request e passar o JWT quando existir
                .AddPolicyHandler(PollyExtensions.EsperarTentar()) //Adicionando um retry policy para tentar 3 vezes antes de retornar erro
                .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30))); //Adicionando um circuit breaker para abrir o circuito por 30 segundos caso ocorra 5 falhas seguidas

            #region Exemplo de implementação do Refit

            //services.AddHttpClient("Refit", options =>
            //{
            //    options.BaseAddress = new Uri(configuration.GetSection("CatalogoUrl").Value);
            //})
            //.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>() //Adicionando o delegating handler para manipular o request e passar o JWT quando existir
            //.AddTypedClient(Refit.RestService.For<ICatalogoServiceRefit>); //Registrando o serviço de catálogo como um HttpClient usando Refit

            #endregion

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUser, AspNetUser>();

            return services;
        }
    }

    public class PollyExtensions
    {
        public static AsyncRetryPolicy<HttpResponseMessage> EsperarTentar()
        {
            var retry = HttpPolicyExtensions.HandleTransientHttpError()
            .WaitAndRetryAsync(new[]
            {
                            TimeSpan.FromSeconds(1),
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(10),
            }, (outcome, timespan, retryCount, context) =>
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"tentando pela {retryCount} vez");
                Console.ForegroundColor = ConsoleColor.White;
            });

            return retry;
        }
    }
}

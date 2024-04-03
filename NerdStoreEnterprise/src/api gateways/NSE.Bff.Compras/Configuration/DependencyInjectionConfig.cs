using NSE.Bff.Compras.Extensions;
using NSE.Bff.Compras.Services;
using NSE.WebApi.Core.Extensions;
using NSE.WebApi.Core.Usuario;
using Polly;


namespace NSE.Bff.Compras.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<IPedidoService, PedidoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>() //Adicionando o delegating handler para manipular o request e passar o JWT quando existir
                .AddPolicyHandler(PollyExtensions.EsperarTentar()) //Adicionando um retry policy para tentar 3 vezes antes de retornar erro
                .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30))); //Adicionando um circuit breaker para abrir o circuito por 30 segundos caso ocorra 5 falhas seguidas;

            services.AddHttpClient<ICatalogoService, CatalogoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>() //Adicionando o delegating handler para manipular o request e passar o JWT quando existir
                .AddPolicyHandler(PollyExtensions.EsperarTentar()) //Adicionando um retry policy para tentar 3 vezes antes de retornar erro
                .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30))); //Adicionando um circuit breaker para abrir o circuito por 30 segundos caso ocorra 5 falhas seguidas

            services.AddHttpClient<ICarrinhoService, CarrinhoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>() //Adicionando o delegating handler para manipular o request e passar o JWT quando existir
                .AddPolicyHandler(PollyExtensions.EsperarTentar()) //Adicionando um retry policy para tentar 3 vezes antes de retornar erro
                .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30))); //Adicionando um circuit breaker para abrir o circuito por 30 segundos caso ocorra 5 falhas seguidas


        }
    }
}

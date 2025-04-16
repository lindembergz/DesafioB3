
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CDB.Domain.Interfaces;
using CDB.Application.Validation;
using CDB.Application.Services;
using CDB.Application.Config;

namespace CalculadoraCDB.Infraestrutura
{
    public static class InjecaoDependencia
    {
        public static IServiceCollection ConfigurarDependencias(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<CalculadoraConfiguration>(
                configuration.GetSection("CalculadoraConfig"));

            services.AddScoped<ICalculadoraInvestimento, CalculadoraInvestimentoService>();

            services.AddScoped<InvestimentoValidator>();

            return services;
        }
    }
}

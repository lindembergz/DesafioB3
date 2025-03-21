using Microsoft.Extensions.DependencyInjection;
using CDB.Domain.Interfaces;
using CDB.Application.Validation;
using CDB.Application.Services;

namespace CalculadoraCDB.Infraestrutura
{
    public static class InjecaoDependencia
    {
        public static IServiceCollection ConfigurarDependencias(this IServiceCollection services)
        {
            services.AddScoped<ICalculadoraInvestimento, CalculadoraInvestimentoService>();

            services.AddScoped<InvestimentoValidator>();

            return services;
        }
    }
}

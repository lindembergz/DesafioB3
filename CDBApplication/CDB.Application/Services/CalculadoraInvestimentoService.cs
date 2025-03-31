using System;
using CDB.Application.Config;
using CDB.Application.Config.CDB.Domain.Configuration;
using CDB.Domain.Interfaces;
using CDB.Domain.Models;
using Microsoft.Extensions.Options;

namespace CDB.Application.Services
{
    public class CalculadoraInvestimentoService : ICalculadoraInvestimento
    {
        private readonly CalculadoraConfiguration _config;
        private const int LIMITE_6_MESES = 6;
        private const int LIMITE_12_MESES = 12;
        private const int LIMITE_24_MESES = 24;
        public CalculadoraInvestimentoService(IOptions<CalculadoraConfiguration> config)
        {
            _config = config.Value ?? throw new ArgumentNullException(nameof(config));
        }

        public ResultadoInvestimento CalcularCDB(decimal valorInicial, int quantidadeMeses)
        {
            decimal valorBruto = valorInicial;

            for (int i = 0; i < quantidadeMeses; i++)
            {
                //Calcula o rendimento mensal com maior precisão
                decimal fatorRendimento = 1 + _config.CDI * _config.TaxaReferencial;

                //Aplica o rendimento ao valor bruto
                valorBruto *= fatorRendimento;

                //Arredonda o valor mensal para evitar acúmulo de erros de ponto flutuante
                valorBruto = Math.Round(valorBruto, 4);
            }

            decimal rendimento = valorBruto - valorInicial;

            decimal aliquotaImposto = DeterminarAliquotaImposto(quantidadeMeses);

            decimal valorImposto = rendimento * aliquotaImposto;

            decimal valorLiquido = valorBruto - valorImposto;

            return new ResultadoInvestimento
            {
                ValorBruto = Math.Round(valorBruto, _config.CasasDecimais),
                ValorLiquido = Math.Round(valorLiquido, _config.CasasDecimais)
            };
        }

        private decimal DeterminarAliquotaImposto(int quantidadeMeses)
        {
            return quantidadeMeses switch
            {
                <= LIMITE_6_MESES => _config.AliquotaAte6Meses,
                <= LIMITE_12_MESES => _config.AliquotaAte12Meses,
                <= LIMITE_24_MESES => _config.AliquotaAte24Meses,
                _ => _config.AliquotaAcima24Meses
            };
        }


    };
}


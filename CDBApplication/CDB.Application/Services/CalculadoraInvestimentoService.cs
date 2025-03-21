using System;
using CDB.Domain.Interfaces;
using CDB.Domain.Models;

namespace CDB.Application.Services
{
    public class CalculadoraInvestimentoService : ICalculadoraInvestimento
    {
        private const decimal TB = 1.08m;//108% convertida para o formato decimal     
        private const decimal CDI = 0.009m;//0,9% 

        private const decimal ALIQUOTA_ATE_6_MESES = 0.225m;//22,5%
        private const decimal ALIQUOTA_ATE_12_MESES = 0.20m;//20%
        private const decimal ALIQUOTA_ATE_24_MESES = 0.175m;//17,5% 
        private const decimal ALIQUOTA_ACIMA_24_MESES = 0.15m;//15% 

        private const int LIMITE_6_MESES = 6;
        private const int LIMITE_12_MESES = 12;
        private const int LIMITE_24_MESES = 24;

        private const int casasDecimais = 2;

        public ResultadoInvestimento CalcularCDB(decimal valorInicial, int quantidadeMeses)
        {
            decimal valorBruto = valorInicial;

            for (int i = 0; i < quantidadeMeses; i++)
            {
                //Calcula o rendimento mensal com maior precisão
                decimal fatorRendimento = 1 + CDI * TB;

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
                ValorBruto = Math.Round(valorBruto, casasDecimais),
                ValorLiquido = Math.Round(valorLiquido, casasDecimais)
            };
        }

        private decimal DeterminarAliquotaImposto(int quantidadeMeses) =>
                  quantidadeMeses switch
                  {
                      <= LIMITE_6_MESES => ALIQUOTA_ATE_6_MESES,
                      <= LIMITE_12_MESES => ALIQUOTA_ATE_12_MESES,
                      <= LIMITE_24_MESES => ALIQUOTA_ATE_24_MESES,
                      _ => ALIQUOTA_ACIMA_24_MESES
                  };
    }
}

using System;
using CDB.Application.Config.CDB.Domain.Configuration;
using System.Diagnostics.CodeAnalysis;
using CDB.Application.Services;
using CDB.Domain.Models;
using Microsoft.Extensions.Options;
using Xunit;

namespace CDB.Tests
{
    [ExcludeFromCodeCoverage]
    public class CalculadoraInvestimentoServiceTests
    {
        private readonly CalculadoraInvestimentoService _calculator;
        private readonly CalculadoraConfiguration _config;

        public CalculadoraInvestimentoServiceTests()
        {
            // Configuração padrão para os testes
            _config = new CalculadoraConfiguration
            {
                TaxaReferencial = 1.08m,
                CDI = 0.009m,
                AliquotaAte6Meses = 0.225m,
                AliquotaAte12Meses = 0.20m,
                AliquotaAte24Meses = 0.175m,
                AliquotaAcima24Meses = 0.15m,
                CasasDecimais = 2
            };
            var options = Options.Create(_config);
            _calculator = new CalculadoraInvestimentoService(options);
        }

        [Theory]
        [InlineData(1000, 3)]  // Até 6 meses
        [InlineData(1000, 9)]  // Até 12 meses
        [InlineData(1000, 18)] // Até 24 meses
        [InlineData(1000, 36)] // Acima de 24 meses
        public void CalcularCDB_ValoresValidos_RetornaResultadosCorretos(decimal valorInicial, int meses)
        {
            // Arrange
            decimal valorBruto = valorInicial;
            decimal fatorRendimento = 1 + _config.CDI * _config.TaxaReferencial;
            for (int i = 0; i < meses; i++)
            {
                valorBruto *= fatorRendimento;
                valorBruto = Math.Round(valorBruto, 4); // Arredondamento mensal
            }
            decimal rendimento = valorBruto - valorInicial;
            decimal aliquota = DeterminarAliquotaImposto(meses);
            decimal imposto = rendimento * aliquota;
            decimal valorLiquidoEsperado = valorBruto - imposto;

            // Act
            ResultadoInvestimento resultado = _calculator.CalcularCDB(valorInicial, meses);

            // Assert
            Assert.Equal(Math.Round(valorBruto, _config.CasasDecimais), resultado.ValorBruto);
            Assert.Equal(Math.Round(valorLiquidoEsperado, _config.CasasDecimais), resultado.ValorLiquido);
        }

        [Fact]
        public void CalcularCDB_ValorInicialZero_RetornaResultadoCorreto()
        {
            // Act
            ResultadoInvestimento resultado = _calculator.CalcularCDB(0, 12);

            // Assert
            Assert.Equal(0m, resultado.ValorBruto);
            Assert.Equal(0m, resultado.ValorLiquido);
        }

        [Fact]
        public void CalcularCDB_UsaTB_e_CDI_Corretamente_ParaUmMes()
        {
            // Arrange
            decimal valorInicial = 1000m;
            int meses = 1;
            decimal fatorRendimentoEsperado = 1 + (_config.CDI * _config.TaxaReferencial);
            decimal valorBrutoEsperado = valorInicial * fatorRendimentoEsperado;
            valorBrutoEsperado = Math.Round(valorBrutoEsperado, 4);
            decimal rendimentoEsperado = valorBrutoEsperado - valorInicial;
            decimal impostoEsperado = rendimentoEsperado * _config.AliquotaAte6Meses; // Até 6 meses
            decimal valorLiquidoEsperado = valorBrutoEsperado - impostoEsperado;

            // Act
            ResultadoInvestimento resultado = _calculator.CalcularCDB(valorInicial, meses);

            // Assert
            Assert.Equal(Math.Round(valorBrutoEsperado, _config.CasasDecimais), resultado.ValorBruto);
            Assert.Equal(Math.Round(valorLiquidoEsperado, _config.CasasDecimais), resultado.ValorLiquido);
        }

        [Theory]
        [InlineData(3, 0.225)]  // Até 6 meses
        [InlineData(9, 0.20)]   // Até 12 meses
        [InlineData(18, 0.175)] // Até 24 meses
        [InlineData(36, 0.15)]  // Acima de 24 meses
        public void CalcularCDB_UsaAliquotaCorreta_ParaDiferentesPrazos(int meses, decimal aliquotaEsperada)
        {
            // Arrange
            decimal valorInicial = 1000m;
            decimal valorBruto = valorInicial;
            decimal fatorRendimento = 1 + _config.CDI * _config.TaxaReferencial;
            for (int i = 0; i < meses; i++)
            {
                valorBruto *= fatorRendimento;
                valorBruto = Math.Round(valorBruto, 4);
            }
            decimal rendimento = valorBruto - valorInicial;
            decimal impostoEsperado = rendimento * aliquotaEsperada;
            decimal valorLiquidoEsperado = valorBruto - impostoEsperado;

            // Act
            ResultadoInvestimento resultado = _calculator.CalcularCDB(valorInicial, meses);

            // Assert
            Assert.Equal(Math.Round(valorBruto, _config.CasasDecimais), resultado.ValorBruto);
            Assert.Equal(Math.Round(valorLiquidoEsperado, _config.CasasDecimais), resultado.ValorLiquido);
        }

        // Método auxiliar para determinar a alíquota nos testes
        private decimal DeterminarAliquotaImposto(int meses)
        {
            if (meses <= 6) return _config.AliquotaAte6Meses;
            if (meses <= 12) return _config.AliquotaAte12Meses;
            if (meses <= 24) return _config.AliquotaAte24Meses;
            return _config.AliquotaAcima24Meses;
        }
    }
}
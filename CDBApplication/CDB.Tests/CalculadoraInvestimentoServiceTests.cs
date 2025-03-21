using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CDB.Application.Services;
using CDB.Domain.Models;
using Xunit;

namespace CDB.Tests
{
    [ExcludeFromCodeCoverage]
    public class CalculadoraInvestimentoServiceTests
    {
        private readonly CalculadoraInvestimentoService _calculator;

        public CalculadoraInvestimentoServiceTests()
        {
            _calculator = new CalculadoraInvestimentoService();
        }

        [Theory]
        [InlineData(1000, 3, 1029.44, 1022.82)] //Até 6 meses: 22,5%
        [InlineData(1000, 9, 1090.96, 1072.77)] //Até 12 meses: 20%
        [InlineData(1000, 18, 1190.19, 1156.91)] //Até 24 meses: 17,5%
        [InlineData(1000, 36, 1416.56, 1354.07)] //Acima de 24 meses: 15%
        public void CalcularCDB_ValoresValidos_RetornaResultadosCorretos(decimal valorInicial, int meses, decimal valorBrutoEsperado, decimal valorLiquidoEsperado)
        {
            
            ResultadoInvestimento resultado = _calculator.CalcularCDB(valorInicial, meses);

            
            Assert.Equal(valorBrutoEsperado, resultado.ValorBruto, 2);
            Assert.Equal(valorLiquidoEsperado, resultado.ValorLiquido, 2);
        }

        [Fact]
        public void CalcularCDB_ValorInicialZero_RetornaResultadoCorreto()
        {
            ResultadoInvestimento resultado = _calculator.CalcularCDB(0, 12);
           
            Assert.Equal(0m, resultado.ValorBruto);
            Assert.Equal(0m, resultado.ValorLiquido);
        }

        [Theory]
        [InlineData(6, 0.225)]  //Até 6 meses
        [InlineData(12, 0.20)]  //Até 12 meses
        [InlineData(24, 0.175)] //Até 24 meses
        [InlineData(25, 0.15)]  //Acima de 24 meses
        public void DeterminarAliquotaImposto_PrazosDiferentes_RetornaAliquotaCorreta(int meses, decimal aliquotaEsperada)
        {
        
            var calculator = new CalculadoraInvestimentoService();
            var methodInfo = typeof(CalculadoraInvestimentoService).GetMethod("DeterminarAliquotaImposto", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

           
            var aliquota = (decimal)methodInfo.Invoke(calculator, new object[] { meses });

           
            Assert.Equal(aliquotaEsperada, aliquota);
        }

        // Teste para verificar se TB (108%) e CDI (0,9%) estão sendo aplicados corretamente
        [Fact]
        public void CalcularCDB_UsaTB_e_CDI_Corretamente_ParaUmMes()
        {

            decimal valorInicial = 1000m;
            int meses = 1;
            decimal fatorRendimentoEsperado = 1 + (0.009m * 1.08m); // CDI * TB
            decimal valorBrutoEsperado = valorInicial * fatorRendimentoEsperado;
            valorBrutoEsperado = Math.Round(valorBrutoEsperado, 4); // Arredondamento usado no código
            decimal rendimentoEsperado = valorBrutoEsperado - valorInicial;
            decimal impostoEsperado = rendimentoEsperado * 0.225m; // Alíquota de até 6 meses
            decimal valorLiquidoEsperado = valorBrutoEsperado - impostoEsperado;

           
            ResultadoInvestimento resultado = _calculator.CalcularCDB(valorInicial, meses);

          
            Assert.Equal(Math.Round(valorBrutoEsperado, 2), resultado.ValorBruto, 2);
            Assert.Equal(Math.Round(valorLiquidoEsperado, 2), resultado.ValorLiquido, 2);
        }


        [Fact]
        public void CalcularCDB_UsaTB_e_CDI_Corretamente_ParaMultiplosMeses()
        {
            
            decimal valorInicial = 1000m;
            int meses = 3;
            decimal valorBruto = valorInicial;
            decimal fatorRendimento = 1 + (0.009m * 1.08m); // CDI * TB
            for (int i = 0; i < meses; i++)
            {
                valorBruto *= fatorRendimento;
                valorBruto = Math.Round(valorBruto, 4); // Arredondamento mensal
            }
            decimal rendimento = valorBruto - valorInicial;
            decimal imposto = rendimento * 0.225m; // Alíquota de até 6 meses
            decimal valorLiquidoEsperado = valorBruto - imposto;

          
            ResultadoInvestimento resultado = _calculator.CalcularCDB(valorInicial, meses);

            
            Assert.Equal(Math.Round(valorBruto, 2), resultado.ValorBruto, 2);
            Assert.Equal(Math.Round(valorLiquidoEsperado, 2), resultado.ValorLiquido, 2);
        }

        //Testes para as alíquotas de imposto
        [Theory]
        [InlineData(3, 0.225, "ALÍQUOTA_ATE_6_MESES")]   //Até 6 meses: 22,5%
        [InlineData(9, 0.20, "ALÍQUOTA_ATE_12_MESES")]   //Até 12 meses: 20%
        [InlineData(18, 0.175, "ALÍQUOTA_ATE_24_MESES")] //Até 24 meses: 17,5%
        [InlineData(36, 0.15, "ALÍQUOTA_ACIMA_24_MESES")] //Acima de 24 meses: 15%
        public void CalcularCDB_UsaAliquotaCorreta_ParaDiferentesPrazos(int meses, decimal aliquotaEsperada, string aliquotaDescricao)
        {
           
            decimal valorInicial = 1000m;
            decimal valorBruto = valorInicial;
            decimal fatorRendimento = 1 + (0.009m * 1.08m); // CDI * TB
            for (int i = 0; i < meses; i++)
            {
                valorBruto *= fatorRendimento;
                valorBruto = Math.Round(valorBruto, 4);
            }
            decimal rendimento = valorBruto - valorInicial;
            decimal impostoEsperado = rendimento * aliquotaEsperada;
            decimal valorLiquidoEsperado = valorBruto - impostoEsperado;

          
            ResultadoInvestimento resultado = _calculator.CalcularCDB(valorInicial, meses);

            Assert.Equal(Math.Round(valorBruto, 2), resultado.ValorBruto, 2);
            Assert.Equal(Math.Round(valorLiquidoEsperado, 2), resultado.ValorLiquido, 2);
        }


        [Fact]
        public void TB_DeveTerValorCorreto()
        {

            var field = typeof(CalculadoraInvestimentoService)
                .GetField("TB", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            decimal valorEsperado = 1.08m;

           
            var valorAtual = (decimal)field.GetValue(null); // null porque é estático

           
            Assert.Equal(valorEsperado, valorAtual);
        }

        [Fact]
        public void CDI_DeveTerValorCorreto()
        {
          
            var field = typeof(CalculadoraInvestimentoService)
                .GetField("CDI", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            decimal valorEsperado = 0.009m;

            
            var valorAtual = (decimal)field.GetValue(null);

         
            Assert.Equal(valorEsperado, valorAtual);
        }

        [Fact]
        public void ALIQUOTA_ATE_6_MESES_DeveTerValorCorreto()
        {
           
            var field = typeof(CalculadoraInvestimentoService)
                .GetField("ALIQUOTA_ATE_6_MESES", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            decimal valorEsperado = 0.225m;

            
            var valorAtual = (decimal)field.GetValue(null);

           
            Assert.Equal(valorEsperado, valorAtual);
        }

        [Fact]
        public void ALIQUOTA_ATE_12_MESES_DeveTerValorCorreto()
        {
            
            var field = typeof(CalculadoraInvestimentoService)
                .GetField("ALIQUOTA_ATE_12_MESES", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            decimal valorEsperado = 0.20m;

            
            var valorAtual = (decimal)field.GetValue(null);

           
            Assert.Equal(valorEsperado, valorAtual);
        }

        [Fact]
        public void ALIQUOTA_ATE_24_MESES_DeveTerValorCorreto()
        {
          
            var field = typeof(CalculadoraInvestimentoService)
                .GetField("ALIQUOTA_ATE_24_MESES", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            decimal valorEsperado = 0.175m;

           
            var valorAtual = (decimal)field.GetValue(null);

           
            Assert.Equal(valorEsperado, valorAtual);
        }


        [Fact]
        public void ALIQUOTA_ATE_ACIMA_24_MESES_DeveTerValorCorreto()
        {

            var field = typeof(CalculadoraInvestimentoService)
                .GetField("ALIQUOTA_ACIMA_24_MESES", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            decimal valorEsperado = 0.15m;


            var valorAtual = (decimal)field.GetValue(null);


            Assert.Equal(valorEsperado, valorAtual);
        }
    }
}

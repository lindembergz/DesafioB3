using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using CDB.Application.DTOs;
using CDB.Application.Validation;
using Xunit;

namespace CDB.Tests
{
    [ExcludeFromCodeCoverage]
    public class InvestimentoValidatorTests
    {
        private readonly InvestimentoValidator _validator;

        public InvestimentoValidatorTests()
        {
            _validator = new InvestimentoValidator();
        }

        [Fact]
        public void ValidarEntrada_EntradaNula_RetornaFalso()
        {
        
            bool resultado = _validator.ValidarEntrada(null, out string mensagemErro);

    
            Assert.False(resultado);
            Assert.Equal("Os dados de entrada são inválidos.", mensagemErro);
        }

        [Theory]
        [InlineData(0, 12, "O valor inicial deve ser positivo.")]
        [InlineData(-1, 12, "O valor inicial deve ser positivo.")]
        [InlineData(0.001, 12, "O valor inicial deve ser pelo menos R$ 0,01.")]
        [InlineData(1000000001, 12, "O valor inicial não pode exceder R$ 1.000.000.000,00.")]
        public void ValidarEntrada_ValorInicialInvalido_RetornaFalso(decimal valorInicial, int meses, string mensagemEsperada)
        {
            
            var entrada = new EntradaInvestimentoDTO { ValorInicial = valorInicial, QuantidadeMeses = meses };

            
            bool resultado = _validator.ValidarEntrada(entrada, out string mensagemErro);

            
            Assert.False(resultado);
            Assert.Equal(mensagemEsperada, mensagemErro);
        }

        [Theory]
        [InlineData(1000, 1, "O prazo deve ser maior que 1 mês.")]
        [InlineData(1000, 241, "O prazo máximo é de 240 meses.")]
        public void ValidarEntrada_PrazoInvalido_RetornaFalso(decimal valorInicial, int meses, string mensagemEsperada)
        {
            
            var entrada = new EntradaInvestimentoDTO { ValorInicial = valorInicial, QuantidadeMeses = meses };

            
            bool resultado = _validator.ValidarEntrada(entrada, out string mensagemErro);

            
            Assert.False(resultado);
            Assert.Equal(mensagemEsperada, mensagemErro);
        }

        [Fact]
        public void ValidarEntrada_DadosValidos_RetornaVerdadeiro()
        {           
            var entrada = new EntradaInvestimentoDTO { ValorInicial = 1000, QuantidadeMeses = 12 };
                        
            bool resultado = _validator.ValidarEntrada(entrada, out string mensagemErro);

            Assert.True(resultado);
            Assert.Empty(mensagemErro);
        }


    }
}
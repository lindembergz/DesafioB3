using System.Diagnostics.CodeAnalysis;
using CDB.API.Controllers;
using CDB.Application.DTOs;
using CDB.Domain.Interfaces;
using CDB.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;


namespace CDB.Tests
{
    [ExcludeFromCodeCoverage]
    public class InvestimentoControllerTests
    {
        private readonly Mock<ICalculadoraInvestimento> _calculatorMock;
        private readonly Mock<ILogger<InvestimentoController>> _loggerMock;
        private readonly InvestimentoController _controller;


        public InvestimentoControllerTests()
        {
            _calculatorMock = new Mock<ICalculadoraInvestimento>();
            _loggerMock = new Mock<ILogger<InvestimentoController>>();
            _controller = new InvestimentoController(_calculatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void CalculateCDB_EntradaInvalida_RetornaBadRequest()
        {
            
            var input = new EntradaInvestimentoDTO { ValorInicial = -1, QuantidadeMeses = 12 };

            
            var result = _controller.CalculateCDB(input);

           
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("O valor inicial deve ser positivo.", badRequestResult.Value);
        }

        [Fact]
        public void CalculateCDB_DadosValidos_RetornaOk()
        {
           
            var input = new EntradaInvestimentoDTO { ValorInicial = 1000, QuantidadeMeses = 12 };
            var resultadoEsperado = new ResultadoInvestimento { ValorBruto = 1088.78m, ValorLiquido = 1078.22m };
            _calculatorMock.Setup(c => c.CalcularCDB(input.ValorInicial, input.QuantidadeMeses)).Returns(resultadoEsperado);

           
            var result = _controller.CalculateCDB(input);

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<ResultadoInvestimentoDTO>(okResult.Value);
            Assert.Equal(resultadoEsperado.ValorBruto, dto.ValorBruto);
            Assert.Equal(resultadoEsperado.ValorLiquido, dto.ValorLiquido);
        }


    }
}
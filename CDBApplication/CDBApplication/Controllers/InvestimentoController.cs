using CDB.Application.DTOs;
using CDB.Domain.Interfaces;
using CDB.Domain.Models;
using CDB.Application.Validation;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CDB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestimentoController : ControllerBase
    {
        private readonly ICalculadoraInvestimento _calculator;
        private readonly InvestimentoValidator _validator;
        private readonly ILogger<InvestimentoController> _logger;

        public InvestimentoController(ICalculadoraInvestimento calculator, ILogger<InvestimentoController> logger)
        {
            _calculator = calculator;
            _validator = new InvestimentoValidator();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("calcular-cdb")]
        public ActionResult<ResultadoInvestimentoDTO> CalculateCDB(EntradaInvestimentoDTO input)
        {
            string mensagemErro;
            if (!_validator.ValidarEntrada(input, out mensagemErro))
            {
                return BadRequest(mensagemErro);
            }

            ResultadoInvestimento resultado = _calculator.CalcularCDB(input.ValorInicial, input.QuantidadeMeses);

            var resultadoDTO = new ResultadoInvestimentoDTO
            {
                ValorBruto = resultado.ValorBruto,
                ValorLiquido = resultado.ValorLiquido
            };

            _logger.LogInformation("Cálculo do CDB realizado com sucesso para ValorInicial: {ValorInicial}, Meses: {QuantidadeMeses}", input.ValorInicial, input.QuantidadeMeses);
            return Ok(resultadoDTO);
        }
    }
}


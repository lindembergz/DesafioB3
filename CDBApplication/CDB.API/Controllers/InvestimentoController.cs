    using Microsoft.AspNetCore.Mvc;
    using CDB.Application.DTOs;
using CDB.Domain.Interfaces;

namespace CDB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestimentoController : ControllerBase
    {
        private readonly ICalculadoraInvestimento _calculator;

        public InvestimentoController(ICalculadoraInvestimento calculator)
        {
            _calculator = calculator;
        }

        [HttpPost("calcular-cdb")]
        public ActionResult<(decimal ValorBruto, decimal ValorLiquido)> CalculateCDB(EntradaInvestimentoDTO input)
        {
            // Validação dos inputs
            /*if (input.ValorInicial <= 0)
                return BadRequest("O valor inicial deve ser positivo.");

            if (input.QuantidadeMeses <= 1)
                return BadRequest("O prazo deve ser maior que 1 mês.");*/

            var result = _calculator.CalcularCDB(input.ValorInicial, input.QuantidadeMeses);

            return Ok(result);
        }
    }
}

using CDB.Application.DTOs;

namespace CDB.Application.Validation
{
    public class InvestimentoValidator
    {
        private const decimal VALOR_MINIMO = 0.01m;
        private const decimal VALOR_MAXIMO = 1000000000m; // 1 bilhão como limite superior
        private const int PRAZO_MINIMO = 2;
        private const int PRAZO_MAXIMO = 240; // 20 anos como limite superior

        public bool ValidarEntrada(EntradaInvestimentoDTO entrada, out string mensagemErro)
        {
            mensagemErro = string.Empty;

            if (entrada == null)
            {
                mensagemErro = "Os dados de entrada são inválidos.";
                return false;
            }

            if (entrada.ValorInicial <= 0)
            {
                mensagemErro = "O valor inicial deve ser positivo.";
                return false;
            }

            if (entrada.ValorInicial < VALOR_MINIMO)
            {
                mensagemErro = $"O valor inicial deve ser pelo menos {VALOR_MINIMO:C}.";
                return false;
            }

            if (entrada.ValorInicial > VALOR_MAXIMO)
            {
                mensagemErro = $"O valor inicial não pode exceder {VALOR_MAXIMO:C}.";
                return false;
            }

            if (entrada.QuantidadeMeses <= 1)
            {
                mensagemErro = "O prazo deve ser maior que 1 mês.";
                return false;
            }

            if (entrada.QuantidadeMeses < PRAZO_MINIMO)
            {
                mensagemErro = $"O prazo mínimo é de {PRAZO_MINIMO} meses.";
                return false;
            }

            if (entrada.QuantidadeMeses > PRAZO_MAXIMO)
            {
                mensagemErro = $"O prazo máximo é de {PRAZO_MAXIMO} meses.";
                return false;
            }

            return true;
        }
    }
}

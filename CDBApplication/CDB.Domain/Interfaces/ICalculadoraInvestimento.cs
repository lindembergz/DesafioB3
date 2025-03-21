using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CDB.Domain.Models;

namespace CDB.Domain.Interfaces
{
    public interface ICalculadoraInvestimento
    {
        ResultadoInvestimento CalcularCDB(decimal valorInicial, int quantidadeMeses);
    }
}

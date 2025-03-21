using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CDB.Application.DTOs
{
    public class EntradaInvestimentoDTO
    {
        [Required(ErrorMessage = "O valor inicial é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor inicial deve ser positivo.")]
        public decimal ValorInicial { get; set; }

        [Required(ErrorMessage = "A quantidade de meses é obrigatória.")]
        [Range(2, 240, ErrorMessage = "O prazo deve ser entre 2 e 240 meses.")]
        public int QuantidadeMeses { get; set; }

    }
}

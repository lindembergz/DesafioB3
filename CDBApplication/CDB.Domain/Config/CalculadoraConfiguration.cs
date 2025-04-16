using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDB.Application.Config
{
        /// <summary>
        /// Configurações para a calculadora de investimentos CDB
        /// </summary>
        public class CalculadoraConfiguration
        {
            /// <summary>
            /// Taxa referencial (TB) - 108% convertido para o formato decimal (1.08)
            /// </summary>
            public decimal TaxaReferencial { get; set; }

            /// <summary>
            /// Taxa CDI (ex: 0.9% representado como 0.009)
            /// </summary>
            public decimal CDI { get; set; }

            /// <summary>
            /// Alíquota de imposto para resgates até 6 meses (22.5% representado como 0.225)
            /// </summary>
            public decimal AliquotaAte6Meses { get; set; }

            /// <summary>
            /// Alíquota de imposto para resgates entre 7 e 12 meses (20% representado como 0.20)
            /// </summary>
            public decimal AliquotaAte12Meses { get; set; }

            /// <summary>
            /// Alíquota de imposto para resgates entre 13 e 24 meses (17.5% representado como 0.175)
            /// </summary>
            public decimal AliquotaAte24Meses { get; set; }

            /// <summary>
            /// Alíquota de imposto para resgates acima de 24 meses (15% representado como 0.15)
            /// </summary>
            public decimal AliquotaAcima24Meses { get; set; }

            /// <summary>
            /// Configurações dos limites de meses para o cálculo das alíquotas
            /// </summary>
              /// <summary>
            /// Número de casas decimais para arredondamento do resultado final
            /// </summary>
            public int CasasDecimais { get; set; }
        }


}

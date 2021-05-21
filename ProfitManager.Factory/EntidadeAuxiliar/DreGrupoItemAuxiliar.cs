using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode.Impl;

namespace ProfitManager.Fabrica.EntidadeAuxiliar
{
    public class DreGrupoItemAuxiliar
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorAcumulado { get; set; }

        public decimal ValorMesAnterior { get; set; }
    }
}

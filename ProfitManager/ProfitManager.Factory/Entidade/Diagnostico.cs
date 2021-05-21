using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.Entidade
{
    public class Diagnostico : EntidadeBase
    {
        public virtual string Codigo { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Texto { get; set; }
        public virtual string TipoDiagnostico { get; set; }
        public virtual DateTime DataAtual { get; set; }
        public virtual DateTime DataInicial { get; set; }
    }
}

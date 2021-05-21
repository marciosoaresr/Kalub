using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.Entidade
{
    public class GrupoDRE: EntidadeBase
    {
        public virtual string Codigo { get; set; }
        public virtual string Nome { get; set; }
        public virtual string tipoDre { get; set; }
        public virtual EnumSimNao Totalizador { get; set; }
        public virtual DateTime DataAtual { get; set; }
        public virtual DateTime DataInicial { get; set; }
        public virtual DateTime DataFinal { get; set; }


    }
}

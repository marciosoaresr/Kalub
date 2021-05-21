using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfitManager.Fabrica.Entidade
{
    public class ExtratoConta: EntidadeBase
    {
        public virtual int IdConta { get; set; }
        public virtual string Codigo { get; set; }
        public virtual Campo CampoNome { get; set; }
        public virtual DateTime DataInicial { get; set; }
        public virtual DateTime DataAtual { get; set; }
        public virtual decimal Saldo { get; set; }
        public virtual int Area { get; set; }
        public virtual string NomeNormalizado
        {
            get { return Codigo + " - " + CampoNome.Nome; }
        }
    }
}

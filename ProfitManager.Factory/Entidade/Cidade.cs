using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfitManager.Fabrica.Entidade
{
    public class Cidade : EntidadeBase
    {
        public virtual int CidadeId { get; set; }
        public virtual string Nome { get; set; }
        public virtual Estado Estado { get; set; }

        public virtual string NomeNormalizado
        {
            get
            {
                return Nome + " - " + Estado.Uf;
            }
        }
    }
}

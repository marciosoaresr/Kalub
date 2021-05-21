using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfitManager.Fabrica.Entidade
{
    public class Estado: EntidadeBase
    {
        public virtual string Nome { get; set; }
        public virtual string Uf { get; set; }
        public virtual Pais Pais { get; set; }

    }
}

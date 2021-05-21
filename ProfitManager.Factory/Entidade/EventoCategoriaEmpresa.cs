using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfitManager.Fabrica.Entidade
{
    public class EventoCategoriaEmpresa: EntidadeBase
    {
        public virtual Evento Evento { get; set; }
        public virtual CategoriaEmpresa CategoriaEmpresa { get; set; }

    }
}

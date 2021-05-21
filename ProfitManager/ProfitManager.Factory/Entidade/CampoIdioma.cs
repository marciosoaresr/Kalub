using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfitManager.Fabrica.Entidade
{
    public class CampoIdioma: EntidadeBase
    {
        public virtual Campo Campo { get; set; }
        public virtual Idioma Idioma { get; set; }
        public virtual string Texto { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.Entidade
{
    public class Parametros : EntidadeBase
    {
        public virtual int DiasGratis { get; set; }
        public virtual decimal ValorPlano { get; set; }
    }
}

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.Entidade
{
    public class TermoUso: EntidadeBase
    {


        [DisplayName("Texto")]

        public virtual string Texto { get; set; }


    }
}

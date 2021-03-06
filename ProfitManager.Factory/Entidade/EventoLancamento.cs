using System;
using System.ComponentModel.DataAnnotations;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.Entidade
{
    public class EventoLancamento: EntidadeBase
    {
        public virtual Empresa Empresa { get; set; }

        [Required(ErrorMessage = "Informe o evento")]
        public virtual Evento Evento { get; set; }

        [Required(ErrorMessage = "Informe a data")]
        public virtual DateTime Data { get; set; }

        [Required(ErrorMessage = "Informe o valor")]
        public virtual decimal Valor { get; set; }

        public virtual string NotaExplicativa { get; set; }

        public virtual string Help { get; set; }

        public virtual decimal ValorMes { get; set; }


    }
}

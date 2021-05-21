using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.Entidade
{
    public class EmpresaRecebimentoTransacao : EntidadeBase
    {
        public virtual string IdTransacao { get; set; }
        public virtual DateTime DataTransacao { get; set; }
        public virtual string TipoPagamento { get; set; }
        public virtual string Status { get; set; }
        public virtual string EmailTransacao { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual decimal Valor { get; set; }
    }
}

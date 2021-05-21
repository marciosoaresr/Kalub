using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.Entidade
{
    public class EmpresaRecebimento : EntidadeBase
    {
        public virtual Empresa Empresa { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual DateTime DataVencimento { get; set; }
        public virtual string Status { get; set; }
        public virtual DateTime? DataRecebimento { get; set; }
        public virtual string IdTransacao { get; set; }
        public virtual string EmailTransacao { get; set; }


    }
}

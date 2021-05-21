using System;
using System.ComponentModel.DataAnnotations;

namespace ProfitManager.Fabrica.Entidade
{
    public class FechamentoCaixa : EntidadeBase
    {
        public virtual TipoDocumentoFechamentoCaixa TipoDocumentoFechamentoCaixa { get; set; }

        public virtual Empresa Empresa { get; set; }

        public virtual DateTime Data { get; set; }

        public virtual decimal Valor { get; set; }


    }
}

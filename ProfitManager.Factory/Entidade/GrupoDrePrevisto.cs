using System;

namespace ProfitManager.Fabrica.Entidade
{
    public class GrupoDrePrevisto : EntidadeBase
    {
        public virtual Empresa Empresa { get; set; }
        public virtual GrupoDRE GrupoDre { get; set; }
        public virtual ContaContabil ContaContabil { get; set; }
        public virtual int Mes { get; set; }
        public virtual decimal Valor { get; set; }
    }
}

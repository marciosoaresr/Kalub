using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Hql.Ast.ANTLR;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.Entidade
{
    public class ContaContabilSaldo: EntidadeBase
    {
        public ContaContabilSaldo()
        {
            ApuracaoResultado = EnumSimNao.Nao;
        }
        public virtual EnumSimNao? ApuracaoResultado { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual ContaContabil ContaContabil { get; set; }
        public virtual EventoLancamento EventoLancamento { get; set; }
        public virtual DateTime Data { get; set; }
        public virtual ContaContabilSaldoInicial ContaContabilSaldoInicial { get; set; }
        public virtual decimal Saldo { get; set; }
    }
}

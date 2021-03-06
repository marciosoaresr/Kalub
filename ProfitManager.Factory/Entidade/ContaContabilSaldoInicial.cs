
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.Entidade
{
    public class ContaContabilSaldoInicial : EntidadeBase
    {
        public virtual string Codigo { get; set; }
        public virtual Campo CampoNome { get; set; }
        public virtual Campo CampoHelp { get; set; }
        public virtual ContaContabil ContaContabil { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual string NotaExplicativaContaContabil { get; set; }
        public virtual string NotaExplicativaGrupo { get; set; }
        public virtual string NotaExplicativaSubGrupo { get; set; }
        public virtual string SubGrupoCodigo { get; set; }
        public virtual string SubGrupoNome { get; set; }
        public virtual string GrupoCodigo { get; set; }
        public virtual string GrupoNome { get; set; }
        public virtual EnumTipoContaContabil TipoContaContabil { get; set; }
        public virtual EnumExigeSaldoinicial ExigeSaldoinicial { get; set; }
        public virtual EnumSimNao LucroPrejuizoAcumulado { get; set; }
        public virtual string Help { get; set; }
    }
}

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.Entidade
{
    public class ContaContabil: EntidadeBase
    {
        [DisplayName("Código")]
        [Required(ErrorMessage = "Informe o código")]
        public virtual string Codigo { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Informe o nome")]
        public virtual Campo CampoNome { get; set; }
        
        [DisplayName("Conta contábil subgrupo")]
        public virtual ContaContabilSubGrupo ContaContabilSubGrupo { get; set; }
        
        [DisplayName("Grupo DRE")]
        public virtual GrupoDRE GrupoDRE { get; set; }

        [DisplayName("Nota Explicativa")]
        public virtual string NotaExplicativaContaContabil { get; set; }
        public virtual string NotaExplicativaGrupo { get; set; }
        public virtual string NotaExplicativaSubGrupo { get; set; }

        [DisplayName("Exige saldo inicial?")]
        public virtual EnumExigeSaldoinicial ExigeSaldoinicial { get; set; }

        [DisplayName("Lucro ou prejuízo acumulado?")]
        public virtual EnumSimNao LucroPrejuizoAcumulado { get; set; }

        [DisplayName("Tipo Conta Contabil")]
        public virtual EnumTipoContaContabil? TipoContaContabil { get; set; }

        [DisplayName("Help")]
        public virtual Campo CampoHelp { get; set; }

        public virtual string SubGrupoCodigo { get; set; }
        public virtual string SubGrupoNome { get; set; }
        public virtual string GrupoCodigo { get; set; }
        public virtual string GrupoNome { get; set; }

        public virtual string Help { get; set; }

        public virtual string NomeNormalizado
        {
            get { return Codigo + " - " + CampoNome.Nome; }
        }


        public override int GetHashCode()
        {
            const int seed = 16777619;
            const int hash = 97;

            int hashCodigo = (this.Codigo == null) ? 0 : this.Codigo.GetHashCode();
            int hashCampoNome = (this.CampoNome == null) ? 0 : this.CampoNome.GetHashCode();
            int hashContaContabilSubGrupo = (this.ContaContabilSubGrupo == null) ? 0 : this.ContaContabilSubGrupo.GetHashCode();

            return seed * (hash + this.Id.GetHashCode() + hashCodigo + hashCampoNome + hashContaContabilSubGrupo);
        }

    }
}

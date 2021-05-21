using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ProfitManager.Fabrica.Enum;


namespace ProfitManager.Fabrica.Entidade
{
    public class ContaContabilSubGrupo: EntidadeBase
    {
        [Required(ErrorMessage = "Informe o código")]
        [DisplayName("Código")]
        public virtual string Codigo { get; set; }
        public virtual Campo CampoNome { get; set; }

        [DisplayName("Grupo contábil")]
        [Required (ErrorMessage = "Informe o grupo de conta contábil")]
        public virtual ContaContabilGrupo ContaContabilGrupo { get; set; }

        [DisplayName("Nota Explicativa")]
        public virtual string NotaExplicativaSubGrupo { get; set; }

        IList<ContaContabil> _ListContaContabil = new List<ContaContabil>();

       public virtual string NomeNormalizado
        {
            get { return Codigo + " - " + CampoNome.Nome; }
        }
        public virtual ContaContabil[] ListContaContabil
        {
            get { return _ListContaContabil.ToArray(); }
        }

        public virtual void AddListContaContabil(ContaContabil item)
        {
            item.ContaContabilSubGrupo = this;
            _ListContaContabil.Add(item);
        }

        public override int GetHashCode()
        {
            const int seed = 16777619;
            const int hash = 97;

            int hashCodigo = (this.Codigo == null) ? 0 : this.Codigo.GetHashCode();
            int hashCampoNome = (this.CampoNome == null) ? 0 : this.CampoNome.GetHashCode();
            int hashContaContabilGrupo = (this.ContaContabilGrupo == null) ? 0 : this.ContaContabilGrupo.GetHashCode();

            return seed * (hash + this.Id.GetHashCode() + hashCodigo + hashCampoNome + hashContaContabilGrupo);
        }

    }

}

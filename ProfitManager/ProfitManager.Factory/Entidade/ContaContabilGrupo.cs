using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.Entidade
{
    public class ContaContabilGrupo: EntidadeBase
    {
        [Required(ErrorMessage = "Informe o código")]
        [DisplayName("Código")]
        public virtual string Codigo { get; set; }
        public virtual EnumContaContabilGrupoTipo? Tipo { get; set; }
        public virtual Campo CampoNome { get; set; }

        [DisplayName("Nota Explicativa")]
        public virtual string NotaExplicativaGrupo { get; set; }

        IList<ContaContabilSubGrupo> _ListContaContabilSubGrupo = new List<ContaContabilSubGrupo>();

        public virtual string NomeNormalizado
        {
            get { return Codigo + " - " + CampoNome.Nome; }
        }

        public virtual ContaContabilSubGrupo[] ListContaContabilSubGrupo
        {
            get { return _ListContaContabilSubGrupo.ToArray(); }
        }

        public virtual void AddListContaContabilSubGrupo(ContaContabilSubGrupo item)
        {
            _ListContaContabilSubGrupo.Add(item);
        }

        public override string ToString()
        {
            return NomeNormalizado;
        }

        public override int GetHashCode()
        {
            const int seed = 16777619;
            const int hash = 97;

            int hashCodigo = (this.Codigo == null) ? 0 : this.Codigo.GetHashCode();
            int hashCampoNome = (this.CampoNome == null) ? 0 : this.CampoNome.GetHashCode();
            int hashTipo = this.Tipo.GetHashCode();

            return seed * (hash + this.Id.GetHashCode() + hashCodigo + hashCampoNome + hashTipo);
        }
    }
}

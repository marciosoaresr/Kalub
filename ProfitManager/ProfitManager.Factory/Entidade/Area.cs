using System;

namespace ProfitManager.Fabrica.Entidade
{
    public class Area: EntidadeBase
    {
        public virtual string Codigo { get; set; }
        public virtual Campo CampoNome { get; set; }
        public virtual string Observacoes { get; set; }
        public virtual GrupoArea GrupoArea { get; set; }

        public virtual string NomeNormalizado
        {
            get { return Codigo + " - " + CampoNome.Nome; }
        }

        public override void SaveValidate()
        {
            if (string.IsNullOrEmpty(Codigo)) throw  new Exception("O código deve ser informado");
        }

        public override int GetHashCode()
        {
            const int seed = 16777619;
            const int hash = 97;

            int hashCodigo = (this.Codigo == null) ? 0 : this.Codigo.GetHashCode();
            int hashCampNome = (this.CampoNome == null) ? 0 : this.CampoNome.GetHashCode();

            return seed * (hash + this.Id.GetHashCode() + hashCodigo + hashCampNome);
        }
    }
}

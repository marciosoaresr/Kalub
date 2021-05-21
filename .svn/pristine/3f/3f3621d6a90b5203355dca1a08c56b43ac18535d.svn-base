using System.ComponentModel.DataAnnotations;

namespace ProfitManager.Fabrica.Entidade
{
    public class Campo: EntidadeBase
    {
        public virtual string Codigo { get; set; }

        [Required(ErrorMessage = "Informe o nome")]
        public virtual string Nome { get; set; }

        public override int GetHashCode()
        {
            const int seed = 16777619;
            const int hash = 97;

            int hashCodigo = (this.Codigo == null) ? 0 : this.Codigo.GetHashCode();
            int hashNome = (this.Nome == null) ? 0 : this.Nome.GetHashCode();

            return seed * (hash + this.Id.GetHashCode() + hashCodigo + hashNome);
        }
    }
}

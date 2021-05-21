using System.ComponentModel.DataAnnotations;

namespace ProfitManager.Fabrica.Entidade
{
    public class ContaContabilRelatorioItem : EntidadeBase
    {
        [Display(Name = "Conta Contábil")]
        public virtual ContaContabil ContaContabil { get; set; }

        [Display(Name = "Relatório Pai")]
        public virtual RelatorioItem RelatorioItem { get; set; }


        public override int GetHashCode()
        {
            const int seed = 16777619;
            const int hash = 97;

            int hashContaContabil = (this.ContaContabil == null) ? 0 : this.ContaContabil.Id.GetHashCode();
            int hashRelatorioItem = (this.RelatorioItem == null) ? 0 : this.RelatorioItem.Id.GetHashCode();

            return seed * (hash + this.Id.GetHashCode() + hashContaContabil + hashRelatorioItem);
        }

    }
}

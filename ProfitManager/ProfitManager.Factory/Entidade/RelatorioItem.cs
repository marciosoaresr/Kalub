using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.Entidade
{
    public class RelatorioItem : EntidadeBase
    {
        [Display(Name = "Código")]
        [Required(ErrorMessage = "Informe o código")]
        public virtual string Codigo { get; set; }

        [Required(ErrorMessage = "Informe o nome")]
        public virtual string Nome { get; set; }

        [Display(Name = "Relatório item parent")]
        public virtual RelatorioItem Parent { get; set; }
        
        [Display(Name = "Formatação condicional")]
        [Required]
        public virtual EnumSimNao Condicional { get; set; }

        [Required]
        public virtual EnumSimNao Negrito { get; set; }


        [Display(Name = "Relatório")]
        [Required(ErrorMessage = "Informe o relatório")]
        public virtual Relatorio Relatorio { get; set; }

        IList<ContaContabilRelatorioItem> _ListContaContabilRelatorioItem = new List<ContaContabilRelatorioItem>();

        public virtual string NomeNormalizado
        {
            get
            {
                return Codigo + " - " + Nome;
            }
        }

        public virtual ContaContabilRelatorioItem[] ListContaContabilRelatorioItem
        {
            get { return _ListContaContabilRelatorioItem.ToArray(); }
        }

        public virtual void AddListContaContabilRelatorioItem(ContaContabilRelatorioItem item)
        {
            if (_ListContaContabilRelatorioItem.Any(x => x.ContaContabil.Id == item.ContaContabil.Id))
                return;

            item.RelatorioItem = this;
            _ListContaContabilRelatorioItem.Add(item);
        }

        public virtual void RemoveItemListContaContabilRelatorioItem(int hashCodeObject)
        {
            if (_ListContaContabilRelatorioItem.All(x => x.GetHashCode() != hashCodeObject))
                return;

            _ListContaContabilRelatorioItem.Remove(
                _ListContaContabilRelatorioItem.First(x => x.GetHashCode() == hashCodeObject));
        }

    }
}

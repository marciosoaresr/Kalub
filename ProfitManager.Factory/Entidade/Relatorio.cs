using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProfitManager.Fabrica.Entidade
{
    public class Relatorio : EntidadeBase
    {
        [DisplayName("Código")]
        [Required(ErrorMessage = "Informe o código")]
        public virtual string Codigo { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Informe o nome")]
        public virtual string Nome { get; set; }


        public virtual string NomeNormalizado
        {
            get
            {
                return Codigo + " - " + Nome;
            }
        }
    }
}

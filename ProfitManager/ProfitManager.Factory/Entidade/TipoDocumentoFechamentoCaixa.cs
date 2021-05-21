using System;
using System.ComponentModel.DataAnnotations;


namespace ProfitManager.Fabrica.Entidade
{
    public class TipoDocumentoFechamentoCaixa : EntidadeBase
    {
        [Required(ErrorMessage = "Informe o código")]
        public virtual string Codigo { get; set; }

        [Required(ErrorMessage = "Informe o nome")]
        public virtual Campo CampoNome { get; set; }
    }
}

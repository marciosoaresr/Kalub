using System;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Web.ViewModels
{
    public class FechamentoCaixaViewModel
    {
        public int IdTipoDocumento { get; set; } 
        public string TipoDocumentoFechamentoCaixa { get; set; }
        public decimal ValorDia { get; set; }
        public DateTime Data { get; set; }
        public int IdEmpresa { get; set; }

    }
}
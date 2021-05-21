using System;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Web.ViewModels
{
    public class EventoLancamentoViewModel
    {
        public int IdEvento { get; set; } 
        public string Evento { get; set; }
        public string EventoHelper { get; set; }
        public string NotaExplicativa { get; set; }
        public EnumTipoEventoLancamento TipoEventoLancamento { get; set; }
        public EnumMaisUsado MaisUsado { get; set; }
        public decimal ValorDia { get; set; }
        public decimal ValorMes { get; set; }
        public decimal TotalValorDia { get; set; }
        public decimal TotalValorMes { get; set; }
        public DateTime Data { get; set; }
        public int IdEmpresa { get; set; }
        public decimal Total { get; set; }
        public Area Area { get; set; }
        public SubArea SubArea { get; set; }
        public string Help { get; set; }
        public int Ordem { get; set; }







    }
}
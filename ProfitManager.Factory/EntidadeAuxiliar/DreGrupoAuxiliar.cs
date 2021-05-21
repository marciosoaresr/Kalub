using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.EntidadeAuxiliar
{
    public class DreGrupoAuxiliar
    {
        public DreGrupoAuxiliar()
        {
            DreGrupoItemAuxiliarList = new List<DreGrupoItemAuxiliar>();
        }
        public DreAuxiliar DreAuxiliar { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public bool Totalizador { get; set; }
        public List<DreGrupoItemAuxiliar> DreGrupoItemAuxiliarList { get; set; }

        public decimal Valor
        {
            get
            {
                if (Totalizador)
                {
                    return
                        DreAuxiliar.DreGrupoAuxiliarList.Where(x => string.CompareOrdinal(x.Codigo , Codigo) < 0 && !x.Totalizador).Sum(x => x.Valor);
                }
                return DreGrupoItemAuxiliarList.Sum(x => x.Valor);
            }
        }
        public decimal ValorAcumulado
        {
            get
            {
                if (Totalizador)
                {
                    return
                        DreAuxiliar.DreGrupoAuxiliarList.Where(x => string.CompareOrdinal(x.Codigo , Codigo) < 0 && !x.Totalizador).Sum(x => x.ValorAcumulado);
                }
                return DreGrupoItemAuxiliarList.Sum(x => x.ValorAcumulado);
            }
        }
        public decimal ValorMesAnterior
        {
            get
            {
                if (Totalizador)
                {
                    return
                        DreAuxiliar.DreGrupoAuxiliarList.Where(x => string.CompareOrdinal(x.Codigo, Codigo) < 0 && !x.Totalizador).Sum(x => x.ValorMesAnterior);
                }
                return DreGrupoItemAuxiliarList.Sum(x => x.ValorMesAnterior);
            }
        }
    }
}

using ProfitManager.Fabrica.Enum;

namespace ProfitManager.Fabrica.EntidadeAuxiliar
{
    public class RelatorioItemAuxiliar
    {
        public int RelatorioItemId { get; set; }
        public int ParentId { get; set; }
        public string CodigoParent { get; set; }
        public string NomeParent { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public EnumSimNao Condicional { get; set; }
        public EnumSimNao Negrito { get; set; }
        public decimal Valor { get; set; }
        public bool HasChild { get; set; }
        public int Profundiade { get; set; }

        public string NomeNormalizado
        {
            get
            {
                return Codigo + " - " + Nome;
            }
        }

        public string NomeNormalizadoParent
        {
            get
            {
                return CodigoParent + " - " + NomeParent;
            }
        }
    }
}

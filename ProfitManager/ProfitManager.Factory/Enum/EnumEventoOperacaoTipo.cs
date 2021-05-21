using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ProfitManager.Fabrica.Enum
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumEventoOperacaoTipo
    {
        Creditar='C', Debitar='D'
    }
}
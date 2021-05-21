using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ProfitManager.Fabrica.Enum
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumSituacaoCadastral
    {
        ATIVA='A', INATIVA='I'
    }
}
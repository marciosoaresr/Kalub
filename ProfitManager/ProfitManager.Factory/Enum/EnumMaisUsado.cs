using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ProfitManager.Fabrica.Enum
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumMaisUsado
    {
        Sim = 'S', Nao = 'N'
    }
}

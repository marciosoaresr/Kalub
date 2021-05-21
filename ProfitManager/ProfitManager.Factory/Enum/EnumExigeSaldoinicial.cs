using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ProfitManager.Fabrica.Enum
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumExigeSaldoinicial
    {
        Sim = 'S', Nao = 'N'
    }
}

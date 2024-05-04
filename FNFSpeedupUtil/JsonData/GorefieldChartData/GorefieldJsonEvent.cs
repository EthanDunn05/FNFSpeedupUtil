using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FNFSpeedupUtil.JsonData.GorefieldChartData;

[JsonObject(MemberSerialization.OptIn)]
public class GorefieldJsonEvent
{
    [JsonProperty("time")]
    public double Time { get; set; }
    
    /// <summary>
    /// Additional data not represented by properties. For example, the song version
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JToken> AdditionalData { get; set; } = new();
}
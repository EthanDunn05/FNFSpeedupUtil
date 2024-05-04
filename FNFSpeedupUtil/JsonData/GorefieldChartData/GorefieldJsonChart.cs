using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FNFSpeedupUtil.JsonData.GorefieldChartData;

[JsonObject(MemberSerialization.OptIn)]
public class GorefieldJsonChart
{
    [JsonProperty("events")]
    public List<GorefieldJsonEvent> Events { get; set; } = new();

    [JsonProperty("scrollSpeed")]
    public double ScrollSpeed { get; set; }

    [JsonProperty("strumLines")]
    public List<GorefieldJsonStrumLine> StrumLines { get; set; } = new();
    
    /// <summary>
    /// Additional data not represented by properties. For example, the song version
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JToken> AdditionalData { get; set; } = new();
}
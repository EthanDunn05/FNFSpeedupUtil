using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FNFSpeedupUtil.JsonData.WeekendChartData;

[JsonObject(MemberSerialization.OptIn)]
public class WeekendJsonNote
{
    [JsonProperty("t")]
    public double Time { get; set; }
    
    [JsonProperty("d")]
    public int Lane { get; set; }
    
    [JsonProperty("l")]
    public double StrumLength { get; set; }
    
    /// <summary>
    /// Additional data not represented by properties
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JToken> AdditionalData { get; set; } = new();
}
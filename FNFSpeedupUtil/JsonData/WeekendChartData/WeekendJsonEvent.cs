using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FNFSpeedupUtil.JsonData.WeekendChartData;

[JsonObject(MemberSerialization.OptIn)]
public class WeekendJsonEvent
{
    /// <summary>
    /// The ms time of the event
    /// </summary>
    [JsonProperty("t")]
    public double Time { get; set; }
    
    /// <summary>
    /// The name of the event
    /// </summary>
    [JsonProperty("e")]
    public string EventName { get; set; }
    
    /// <summary>
    /// The arguments of the event. Can vary wildly between event types
    /// </summary>
    [JsonProperty("v")]
    public JToken Arguments { get; set; }
    
    /// <summary>
    /// Additional data not represented by properties
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JToken> AdditionalData { get; set; } = new();
}
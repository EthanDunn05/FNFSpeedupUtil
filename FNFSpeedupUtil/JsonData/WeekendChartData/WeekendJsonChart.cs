using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FNFSpeedupUtil.JsonData.WeekendChartData;

[JsonObject(MemberSerialization.OptIn)]
public class WeekendJsonChart
{
    /// <summary>
    /// The list of events
    /// </summary>
    [JsonProperty("events")]
    public List<WeekendJsonEvent> Events { get; set; }
    
    /// <summary>
    /// The notes seperated for each difficulty
    /// </summary>
    [JsonProperty("notes")]
    public Dictionary<string, List<WeekendJsonNote>> Notes { get; set; }
    
    /// <summary>
    /// Additional data not represented by properties. For example, the song version
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JToken> AdditionalData { get; set; } = new();
}
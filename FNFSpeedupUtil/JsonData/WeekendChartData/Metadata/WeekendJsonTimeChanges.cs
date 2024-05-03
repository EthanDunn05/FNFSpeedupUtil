using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FNFSpeedupUtil.JsonData.WeekendChartData.Metadata;

[JsonObject(MemberSerialization.OptIn)]
public class WeekendJsonTimeChanges
{
    /// <summary>
    /// The bpm of this time change
    /// </summary>
    [JsonProperty("bpm")]
    public double Bpm { get; set; }
    
    /// <summary>
    /// When this time change is
    /// </summary>
    [JsonProperty("t")]
    public double Time { get; set; }
    
    /// <summary>
    /// Additional data not represented by properties.
    /// There are a few properties that I have no idea what they are. Like "d" and "n"
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JToken> AdditionalData { get; set; } = new();
}
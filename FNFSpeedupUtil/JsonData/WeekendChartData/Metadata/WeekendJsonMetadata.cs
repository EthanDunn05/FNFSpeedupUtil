using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FNFSpeedupUtil.JsonData.WeekendChartData.Metadata;

[JsonObject(MemberSerialization.OptIn)]
public class WeekendJsonMetadata
{
    /// <summary>
    /// FINALLY THEY STORE THE SONG NAME WITH THE SONG DATA!
    /// </summary>
    [JsonProperty("songName")]
    public string Name { get; set; }

    [JsonProperty("timeChanges")]
    public List<WeekendJsonTimeChanges> TimeChanges { get; set; } = new();
    
    /// <summary>
    /// Additional data not represented by properties. This holds a lot of data that we don't care about.
    /// Like, we know the time format is in ms.
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JToken> AdditionalData { get; set; } = new();
}
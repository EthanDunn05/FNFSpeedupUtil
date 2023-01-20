using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FNFSpeedupUtil.ChartData;

/// <summary>
/// A representation of the "song" object in the chart.
/// Has general data about the chart as a whole.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public class JsonSong
{
    /// <summary>
    /// A list of all the sections in the chart.
    /// </summary>
    [JsonProperty("notes")]
    public List<JsonSection> Sections { get; set; }

    /// <summary>
    /// The scroll speed of the chart.
    /// </summary>
    [JsonProperty("speed")]
    public double Speed { get; set; }

    /// <summary>
    /// The bpm of the chart.
    /// </summary>
    [JsonProperty("bpm")]
    public double Bpm { get; set; }
    
    /// <summary>
    /// A list of all the events in the chart.
    /// Possibly null as not all charts have this property.
    /// </summary>
    [JsonProperty("events")]
    public List<JsonEvent>? Events { get; set; }
    
    /// <summary>
    /// Hold the data not represented by a property so that it isn't lost when modifying the chart.
    /// Often mods have more chart data specific to the mod so this holds all of that data.
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JToken> AdditionalData { get; set; }
}
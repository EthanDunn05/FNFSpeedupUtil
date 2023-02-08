using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FNFSpeedupUtil.JsonData.ChartData;

/// <summary>
/// The root object of a chart file.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public class JsonChart
{
    /// <summary>
    /// The song object... It holds all the data...
    /// </summary>
    [JsonProperty("song")]
    public JsonSong Song { get; set; }
    
    /// <summary>
    /// Additional data not represented by properties. Should always
    /// be empty, but this is here so that data will never be lost.
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JToken> AdditionalData { get; set; }
}
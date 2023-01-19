using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FNFSpeedupUtil.ChartData;

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

    public static JsonSong Deserialize(string chartPath)
    {
        var chartText = File.ReadAllText(chartPath);
        return JsonConvert.DeserializeObject<JsonSong>(chartText) ?? throw new InvalidOperationException();
    }

    public void Serialize(string chartPath)
    {
        var chartText = JsonConvert.SerializeObject(this);
        File.WriteAllText(chartPath, chartText);
    }
}
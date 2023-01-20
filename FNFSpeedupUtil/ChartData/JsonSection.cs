using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FNFSpeedupUtil.ChartData;

/// <summary>
/// Represents one section of a chart. Mainly used to hold notes, but might contain
/// other useful information.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public class JsonSection
{
    /// <summary>
    /// A list of the notes in this section.
    /// </summary>
    [JsonProperty("sectionNotes")]
    public List<JsonNote> SectionNotes { get; set; }
    
    /// <summary>
    /// The section bpm.
    /// </summary>
    [JsonProperty("bpm")]
    public double? Bpm { get; set; }
    
    /// <summary>
    /// Hold the extra data since we can't be removing data
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JToken> AdditionalData { get; set; }
}
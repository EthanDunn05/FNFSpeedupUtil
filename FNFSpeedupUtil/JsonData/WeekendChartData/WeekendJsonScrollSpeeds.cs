using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FNFSpeedupUtil.JsonData.WeekendChartData;

[JsonObject(MemberSerialization.OptIn)]
public class WeekendJsonScrollSpeeds
{
    [JsonExtensionData]
    public Dictionary<string, JToken> ScrollSpeeds { get; set; } = new();
}
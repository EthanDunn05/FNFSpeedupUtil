using Newtonsoft.Json;

namespace FNFSpeedupUtil.JsonData.WeekendChartData;

[JsonObject(MemberSerialization.OptIn)]
public class WeekendJsonScrollSpeeds
{
    [JsonExtensionData]
    public Dictionary<string, double> ScrollSpeeds { get; set; } = new();
}
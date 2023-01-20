using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FNFSpeedupUtil.ChartData;

/// <summary>
/// Event data is stored as an array in the chart json, so this class
/// is an IEnumerable to represent that.
/// </summary>
[JsonArray]
public class JsonEvent : List<JValue>
{
    /// <summary>
    /// The time the event triggers
    /// </summary>
    public double EventTime
    {
        get => (double)this[0];
        set => this[0] = (JValue)value;
    }

    public IEnumerator GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FNFSpeedupUtil.ChartData;

/// <summary>
/// Note data is stored as an array in the chart json, so this class
/// is an IEnumerable to represent that.
/// </summary>
[JsonArray]
public class JsonNote : IEnumerable
{
    private List<JValue> _data = new();

    /// <summary>
    /// The time the note is to be hit.
    /// </summary>
    public double NoteTime
    {
        get => (double)_data[0];
        set => _data[0] = (JValue)value;
    }

    /// <summary>
    /// The position of the note on the strumbar.
    /// </summary>
    public int StrumPos
    {
        get => (int)_data[1];
        set => _data[1] = (JValue)value;
    }

    /// <summary>
    /// Weather or not the not is an event note.
    /// (Might not always be accurate because some mods do things in different ways)
    /// </summary>
    public bool IsEventNote => StrumPos == -1;

    /// <summary>
    /// The length of the sustain. Will be 0 if the note is not a sustain.
    /// </summary>
    public double SustainLength
    {
        // Might not be a double such as in an event, so error check the cast
        get => (double?)_data[2] ?? throw new InvalidCastException();
        set => _data[2] = (JValue)value;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _data.GetEnumerator();
    }
}
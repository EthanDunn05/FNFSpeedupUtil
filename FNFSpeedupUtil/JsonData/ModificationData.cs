using System.IO.Abstractions;
using Newtonsoft.Json;

namespace FNFSpeedupUtil.JsonData;

/// <summary>
/// Represents the data that will show what changes have been done to a song.
/// Will be serialized and deserialized to and from JSON.
/// </summary>
[JsonObject(MemberSerialization.OptOut)]
public class ModificationData
{
    /// <summary>
    /// Represents the current speed modifier applied to the song.
    /// </summary>
    public double SpeedModifier { get; set; } = 1;
}
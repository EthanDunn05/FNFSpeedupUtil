using System.ComponentModel;
using System.IO.Abstractions;
using Newtonsoft.Json;

namespace FNFSpeedupUtil;

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

    /// <summary>
    /// Deserializes the modification data file.
    /// </summary>
    /// <param name="dataFile">The path to the modification data json file</param>
    /// <returns>The deserialized contents of the inputted file</returns>
    /// <exception cref="InvalidDataException">
    /// Thrown if the data path exists, but could not be deserialized into the ModificationData type
    /// </exception>
    public static ModificationData Deserialize(IFileInfo dataFile)
    {
        var fileSystem = dataFile.FileSystem;
        var fileContents = fileSystem.File.ReadAllText(dataFile.FullName);

        // Funky syntax for type checking the deserialized object
        var deserialized = JsonConvert.DeserializeObject<ModificationData>(fileContents);
        if (deserialized == null)
        {
            throw new InvalidDataException($"{dataFile} is not a valid modification data file");
        }

        return deserialized;
    }

    /// <summary>
    /// Serializes this modification data and writes it to the dataPath.
    /// </summary>
    /// <param name="dataFile">The path the modification data should write to</param>
    public void Serialize(IFileInfo dataFile)
    {
        var serialized = JsonConvert.SerializeObject(this);

        var fileSystem = dataFile.FileSystem;
        fileSystem.File.WriteAllText(dataFile.FullName, serialized);
    }
}
using System.IO.Abstractions;
using Newtonsoft.Json;

namespace FNFSpeedupUtil.Extensions;

/// <summary>
/// Contains methods to help with json serialization where JsonConvert can't
/// </summary>
public static class FileExtensions
{
    /// <summary>
    /// Deserializes a json file to a given type.
    /// </summary>
    /// <param name="jsonFile">The file to read from</param>
    /// <typeparam name="T">The type of the object to deserialize</typeparam>
    /// <returns>A new object of type T</returns>
    /// <exception cref="InvalidDataException">Thrown when the file is not a valid json file.</exception>
    public static T DeserializeJson<T>(this IFileInfo jsonFile)
    {
        var fileSystem = jsonFile.FileSystem;
        var fileContents = fileSystem.File.ReadAllText(jsonFile.FullName);

        var deserialized = JsonConvert.DeserializeObject<T>(fileContents);
        
        if (deserialized == null)
            throw new InvalidDataException($"{jsonFile} is not a valid json file");

        return deserialized;
    }

    /// <summary>
    /// Serializes the given object to a file.
    /// </summary>
    /// <param name="toSerialize">The object that is being serialized</param>
    /// <param name="outputFile">
    /// The file to write to. If the file does not exist, it will be created.
    /// If the file already exists, it will be overriden
    /// </param>
    public static void SerializeJson(this IFileInfo outputFile, object toSerialize)
    {
        var serialized = JsonConvert.SerializeObject(toSerialize);

        var fileSystem = outputFile.FileSystem;
        fileSystem.File.WriteAllText(outputFile.FullName, serialized);
    }
}
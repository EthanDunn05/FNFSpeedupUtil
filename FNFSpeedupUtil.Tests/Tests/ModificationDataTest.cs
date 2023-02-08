using System.IO.Abstractions.TestingHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FNFSpeedupUtil.Tests.Tests;

public class ModificationDataTest
{
    private MockFileSystem MockFs { get; }
    private MockFileInfo ModificationDataFile { get; }

    public ModificationDataTest()
    {
        // Create a mock file system with a mod and test song
        MockFs = new MockFileSystem();
        ModificationDataFile = new MockFileInfo(MockFs, "mock-modification-data.json");
        ModificationDataFile.Create();
    }

    [Fact]
    public void Deserialize_CustomData_ReadsCorrectly()
    {
        // Arrange
        const string data = @"
{
    ""SpeedModifier"": 2
}";
        MockFs.File.WriteAllText(ModificationDataFile.FullName, data);

        // Act
        var testData = ModificationData.Deserialize(ModificationDataFile);

        // Assert
        Assert.Equal(2, testData.SpeedModifier);
    }

    [Fact]
    public void Deserialize_NoData_UsesDefaultValues()
    {
        // Arrange
        MockFs.File.WriteAllText(ModificationDataFile.FullName, "{}");

        // Act
        var testData = ModificationData.Deserialize(ModificationDataFile);

        // Assert
        Assert.Equal(1, testData.SpeedModifier);
    }

    [Fact]
    public void Deserialize_InvalidJson_ThrowsError()
    {
        Assert.Throws<InvalidDataException>(() =>
        {
            ModificationData.Deserialize(ModificationDataFile);
        });
    }

    [Fact]
    public void Serialize_Data_WritesCorrectly()
    {
        // Arrange
        var expectedData = new ModificationData { SpeedModifier = 2 };

        // Act
        expectedData.Serialize(ModificationDataFile);
        var fileContents = MockFs.File.ReadAllText(ModificationDataFile.FullName);
        var deserializedData = JObject.Parse(fileContents);

        // Assert
        Assert.Equal(expectedData.SpeedModifier, deserializedData["SpeedModifier"]);
    }
}
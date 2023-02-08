using System.IO.Abstractions.TestingHelpers;
using FNFSpeedupUtil.Extensions;
using Newtonsoft.Json;

namespace FNFSpeedupUtil.Tests.Tests;

public class FileExtensionsTest
{
    [Fact]
    public void DeserializeJson_Object_ReadsCorrectly()
    {
        // Arrange
        var mockFs = new MockFileSystem();
        var testFile = new MockFileInfo(mockFs, "testFile.json");

        var expectedObject = new TestObject();
        var jsonText = JsonConvert.SerializeObject(expectedObject);
        mockFs.File.WriteAllText(testFile.FullName, jsonText);
        
        // Act
        var testObject = testFile.DeserializeJson<TestObject>();

        // Assert
        Assert.Equal(expectedObject.TestBool, testObject.TestBool);
        Assert.Equal(expectedObject.TestInt, testObject.TestInt);
    }

    [Fact]
    public void SerializeJson_Object_WriteCorrectly()
    {
        // Arrange
        var mockFs = new MockFileSystem();
        var testFile = new MockFileInfo(mockFs, "testFile.json");
        
        var testObject = new TestObject();
        var expectedText = JsonConvert.SerializeObject(testObject);
        
        // Act
        testFile.SerializeJson(testObject);
        var testText = mockFs.File.ReadAllText(testFile.FullName);

        // Assert
        Assert.Equal(expectedText, testText);
    }
    
    [JsonObject]
    private class TestObject
    {
        public bool TestBool = true;
        public int TestInt = 1;
    }
}
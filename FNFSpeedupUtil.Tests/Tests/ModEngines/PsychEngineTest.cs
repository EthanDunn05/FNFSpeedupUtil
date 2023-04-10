using System.IO.Abstractions.TestingHelpers;
using FNFSpeedupUtil.ModEngines;

namespace FNFSpeedupUtil.Tests.Tests.ModEngines;

public class PsychEngineTest
{
    [Fact]
    public void ValidForMod_Valid_ReturnsTrue()
    {
        // Assemble
        var mockFs = MakeModFs();
        mockFs.AddDirectory("C:/mods/data/test");
        mockFs.AddDirectory("C:/mods/songs/test");
        var root = new MockDirectoryInfo(mockFs, "C:/");
        var engine = new PsychEngine();

        // Assert
        Assert.True(engine.ValidForMod(root));
    }
    
    [Fact]
    public void ValidForMod_NoSongs_ReturnsFalse()
    {
        // Assemble
        var mockFs = MakeModFs();
        var root = new MockDirectoryInfo(mockFs, "C:/");
        var engine = new PsychEngine();

        // Assert
        Assert.False(engine.ValidForMod(root));
    }

    [Fact]
    public void ValidForMod_Invalid_ReturnsFalse()
    {
        // Assemble
        var mockFs = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { "C:/", new MockDirectoryData() }
        });
        var root = new MockDirectoryInfo(mockFs, "C:/");
        var engine = new PsychEngine();

        // Assert
        Assert.False(engine.ValidForMod(root));
    }

    [Fact]
    public void FindSongs_CorrectlyFinds()
    {
        // Assemble
        var mockFs = MakeModFs();
        mockFs.AddDirectory("C:/mods/data/test");
        mockFs.AddDirectory("C:/mods/songs/test");
        var root = new MockDirectoryInfo(mockFs, "C:/");
        var engine = new PsychEngine();
        
        // Act
        var songs = engine.FindSongs(root);

        // Assert
        var foundSong = songs.Any(s => s.Name == "test");
        Assert.True(foundSong);
    }

    private static MockFileSystem MakeModFs() =>
        new(new Dictionary<string, MockFileData>
        {
            { "C:/", new MockDirectoryData() },
            { "C:/mods", new MockDirectoryData() },
            { "C:/mods/songs", new MockDirectoryData() },
            { "C:/mods/data", new MockDirectoryData() }
        });
}
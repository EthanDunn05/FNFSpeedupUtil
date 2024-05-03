using System.IO.Abstractions.TestingHelpers;
using FNFSpeedupUtil.ModEngines;
using FNFSpeedupUtil.SongManagement;

namespace FNFSpeedupUtil.Tests.Tests.ModEngines;

public class VanillaEngineTest
{
    [Fact]
    public void ValidForMod_Valid_ReturnsTrue()
    {
        // Assemble
        var mockFs = MakeModFs();
        mockFs.AddDirectory("C:/assets/data/test");
        mockFs.AddDirectory("C:/assets/songs/test");
        var root = new MockDirectoryInfo(mockFs, "C:/");
        var engine = new OgVanillaEngine();

        // Assert
        Assert.True(engine.ValidForMod(root));
    }
    
    [Fact]
    public void ValidForMod_NoSongs_ReturnsFalse()
    {
        // Assemble
        var mockFs = MakeModFs();
        var root = new MockDirectoryInfo(mockFs, "C:/");
        var engine = new OgVanillaEngine();

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
        var engine = new OgVanillaEngine();

        // Assert
        Assert.False(engine.ValidForMod(root));
    }

    [Fact]
    public void FindSongs_CorrectlyFinds()
    {
        // Assemble
        var mockFs = MakeModFs();
        mockFs.AddDirectory("C:/assets/data/test");
        mockFs.AddDirectory("C:/assets/songs/test");
        var root = new MockDirectoryInfo(mockFs, "C:/");
        var engine = new OgVanillaEngine();
        
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
            { "C:/assets", new MockDirectoryData() },
            { "C:/assets/songs", new MockDirectoryData() },
            { "C:/assets/data", new MockDirectoryData() }
        });
}
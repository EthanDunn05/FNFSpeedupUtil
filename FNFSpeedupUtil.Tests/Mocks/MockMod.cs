using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace FNFSpeedupUtil.Tests.Mocks;

public class MockMod
{
    public MockDirectoryInfo ModDir { get; }
    public MockDirectoryInfo AssetsDir { get; }
    public MockDirectoryInfo DataDir { get; }
    public MockDirectoryInfo SongsDir { get; }

    /// <summary>
    /// Creates the file system containing only a mod of the given name. The mod has the standard scaffolding for a mod
    /// using asset files.
    /// </summary>
    /// <param name="modName">The name of the mod to mock</param>
    public MockMod(MockDirectoryInfo modDir)
    {
        ModDir = modDir;
        AssetsDir = (MockDirectoryInfo)ModDir.CreateSubdirectory("assets");
        DataDir = (MockDirectoryInfo)AssetsDir.CreateSubdirectory("data");
        SongsDir = (MockDirectoryInfo)AssetsDir.CreateSubdirectory("songs");
    }

    public MockSong CreateSong(string songName)
    {
        return new MockSong(this, songName);
    }
}
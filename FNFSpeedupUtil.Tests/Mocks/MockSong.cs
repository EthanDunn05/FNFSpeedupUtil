using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using FNFSpeedupUtil.Extensions;
using FNFSpeedupUtil.JsonData.ChartData;

namespace FNFSpeedupUtil.Tests.Mocks;

public class MockSong
{
    private MockMod Mod { get; }
    public string Name { get; }
    
    public MockDirectoryInfo DataDir { get; }
    public MockDirectoryInfo SongDir { get; }

    public MockSong(MockMod mod, string name)
    {
        Mod = mod;
        Name = name;

        DataDir = (MockDirectoryInfo)Mod.DataDir.CreateSubdirectory(name);
        SongDir = (MockDirectoryInfo)Mod.SongsDir.CreateSubdirectory(name);
    }

    public MockFileInfo AddDifficulty(string difficultyName, JsonChart chart)
    {
        var fileName = difficultyName != ""
            ? $"{Name}-{difficultyName}.json"
            : $"{Name}.json";
        
        var chartFile = DataDir.File(fileName);
        chartFile.Create();
        chartFile.SerializeJson(chart);
        return (MockFileInfo)chartFile;
    }

    public MockFileInfo AddEvents(JsonChart chart)
    {
        var chartFile = DataDir.File("events.json");
        chartFile.Create();
        chartFile.SerializeJson(chart);
        return (MockFileInfo)chartFile;
    }

    public MockFileInfo AddInst()
    {
        var instFile = SongDir.File("Inst.ogg");
        instFile.Create();
        return (MockFileInfo)instFile;
    }

    public MockFileInfo AddVoices()
    {
        var voicesFile = SongDir.File("Voices.ogg");
        voicesFile.Create();
        return (MockFileInfo)voicesFile;
    }

    public Song MakeSong() => new Song(Name, DataDir, SongDir);
}
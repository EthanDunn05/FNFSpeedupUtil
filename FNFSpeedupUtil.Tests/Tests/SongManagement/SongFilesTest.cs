using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using FNFSpeedupUtil.Extensions;
using FNFSpeedupUtil.JsonData.OgChartData;
using FNFSpeedupUtil.SongManagement;

namespace FNFSpeedupUtil.Tests.Tests.SongManagement;

public class SongFilesTest
{
    private const string UtilDirName = "SpeedupUtilFiles";
    private const string BackupDataDirName = "backupData";
    private const string BackupSongDirName = "backupSong";

    [Fact]
    public void Constructor_Parameters_SetCorrectly()
    {
        // Arrange
        var fs = new MockFileSystem();
        var mod = MakeModFolder(fs, "testMod");
        var (data, songs) = MakeSong(mod, "testSong");

        // Act
        var testSong = new OgSongFiles("testSong", data, songs);

        // Assert
        Assert.Equal("testSong", testSong.Name);
        Assert.Equal(data, testSong.DataFolder);
        Assert.Equal(songs, testSong.MusicFolder);
    }

    [Fact]
    public void Constructor_Difficulties_FoundAll()
    {
        // Arrange
        var fs = new MockFileSystem();
        var mod = MakeModFolder(fs, "testMod");
        var (data, songs) = MakeSong(mod, "testSong");

        var expectedDiffs = new List<string>
        {
            AddDifficulty(data, "test1", new OgJsonChart()).FullName,
            AddDifficulty(data, "test2", new OgJsonChart()).FullName,
            AddDifficulty(data, "", new OgJsonChart()).FullName
        };

        // Act
        var testSong = new OgSongFiles("testSong", data, songs);
        var testDiffs = testSong.DifficultyFiles.Select(f => f.FullName);

        // Assert
        Assert.Equal(expectedDiffs, testDiffs);
    }

    [Fact]
    public void Constructor_Events_Found()
    {
        // Arrange
        var fs = new MockFileSystem();
        var mod = MakeModFolder(fs, "testMod");
        var (data, songs) = MakeSong(mod, "testSong");
        var expectedPath = AddEvents(data, new OgJsonChart()).FullName;

        // Act
        var testSong = new OgSongFiles("testSong", data, songs);

        // Assert
        Assert.NotNull(testSong.EventsFile);
        Assert.Equal(expectedPath, testSong.EventsFile!.FullName);
    }

    [Fact]
    public void Constructor_SongFiles_Found()
    {
        // Arrange
        var fs = new MockFileSystem();
        var mod = MakeModFolder(fs, "testMod");
        var (data, songs) = MakeSong(mod, "testSong");
        
        var expectedInstPath = AddInst(songs).FullName;
        var expectedVoicesPath = AddVoices(songs).FullName;

        // Act
        var testSong = new OgSongFiles("testSong", data, songs);

        // Assert
        Assert.Equal(expectedInstPath, testSong.InstFile.FullName);
        Assert.Equal(expectedVoicesPath, testSong.VoicesFile.FullName);
    }

    [Fact]
    public void Constructor_UtilityFile_Created()
    {
        // Arrange
        var fs = new MockFileSystem();
        var mod = MakeModFolder(fs, "testMod");
        var (data, songs) = MakeSong(mod, "testSong");
        
        // Act
        var testSong = new OgSongFiles("testSong", data, songs);
        var mockUtilityFile = data.SubDirectory(UtilDirName);

        // Assert
        Assert.True(mockUtilityFile.Exists);
    }

    [Fact]
    public void Constructor_ModificationData_Created()
    {
        // Arrange
        var fs = new MockFileSystem();
        var mod = MakeModFolder(fs, "testMod");
        var (data, songs) = MakeSong(mod, "testSong");
        
        // Act
        new OgSongFiles("testSong", data, songs);
        var mockModificationData = data
            .SubDirectory(UtilDirName)
            .File("modification-data.json");

        // Assert
        Assert.True(mockModificationData.Exists);
    }

    [Fact]
    public void Constructor_Data_CreatedBackup()
    {
        // Arrange
        var fs = new MockFileSystem();
        var mod = MakeModFolder(fs, "testMod");
        var (data, songs) = MakeSong(mod, "testSong");
        
        AddDifficulty(data, "test", new OgJsonChart());
        AddEvents(data, new OgJsonChart());

        // Act
        var testSong = new OgSongFiles("testSong", data, songs);

        var backupDir = data
            .SubDirectory(UtilDirName)
            .SubDirectory(BackupDataDirName);

        var backupDifficulty = backupDir.File("testSong-test.json");
        var backupEvents = backupDir.File("events.json");

        // Assert
        Assert.True(backupDir.Exists);
        Assert.True(backupDifficulty.Exists);
        Assert.True(backupEvents.Exists);
    }

    [Fact]
    public void Constructor_Song_CreatedBackup()
    {
        // Arrange
        var fs = new MockFileSystem();
        var mod = MakeModFolder(fs, "testMod");
        var (data, songs) = MakeSong(mod, "testSong");
        
        AddInst(songs);
        AddVoices(songs);

        // Act
        var testSong = new OgSongFiles("testSong", data, songs);

        var backupDir = data
            .SubDirectory(UtilDirName)
            .SubDirectory(BackupSongDirName);

        var backupInst = backupDir.File("Inst.ogg");
        var backupVoices = backupDir.File("Voices.ogg");

        // Assert
        Assert.True(backupDir.Exists);
        Assert.True(backupInst.Exists);
        Assert.True(backupVoices.Exists);
    }

    private IDirectoryInfo MakeModFolder(IMockFileDataAccessor fs, string name)
    {
        var modDir = new MockDirectoryInfo(fs, name);
        modDir.Create();
        var assetsDir = modDir.CreateSubdirectory("assets");
        assetsDir.CreateSubdirectory("data");
        assetsDir.CreateSubdirectory("songs");

        return modDir;
    }

    private (IDirectoryInfo data, IDirectoryInfo songs) MakeSong(IDirectoryInfo mod, string name)
    {
        return (
            mod.SubDirectory("data").CreateSubdirectory(name),
            mod.SubDirectory("songs").CreateSubdirectory(name)
        );
    }
    
    private IFileInfo AddDifficulty(IDirectoryInfo dataDir, string difficultyName, OgJsonChart chart)
    {
        var name = dataDir.Name;
        var fileName = difficultyName != ""
            ? $"{name}-{difficultyName}.json"
            : $"{name}.json";
        
        var chartFile = dataDir.File(fileName);
        chartFile.Create();
        chartFile.SerializeJson(chart);
        return chartFile;
    }
    
    private MockFileInfo AddEvents(IDirectoryInfo dataDir, OgJsonChart chart)
    {
        var chartFile = dataDir.File("events.json");
        chartFile.Create();
        chartFile.SerializeJson(chart);
        return (MockFileInfo)chartFile;
    }

    private MockFileInfo AddInst(IDirectoryInfo songDir)
    {
        var instFile = songDir.File("Inst.ogg");
        instFile.Create();
        return (MockFileInfo)instFile;
    }

    private MockFileInfo AddVoices(IDirectoryInfo songDir)
    {
        var voicesFile = songDir.File("Voices.ogg");
        voicesFile.Create();
        return (MockFileInfo)voicesFile;
    }
}
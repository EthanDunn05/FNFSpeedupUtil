using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using FNFSpeedupUtil.JsonData.ChartData;
using FNFSpeedupUtil.Tests.Mocks;

namespace FNFSpeedupUtil.Tests.Tests.SongManagement;

public class SongFilesTest
{
    private MockSong MockSong { get; }
    private const string UtilDirName = "SpeedupUtilFiles";
    private const string BackupDataDirName = "backupData";
    private const string BackupSongDirName = "backupSong";

    public SongFilesTest()
    {
        // Create a mock file system with a mod and test song
        var mockFs = new MockFileSystem();
        var modDir = new MockDirectoryInfo(mockFs, "testMod");
        var mockMod = new MockMod(modDir);
        MockSong = mockMod.CreateSong("testSong");
    }

    [Fact]
    public void Constructor_Parameters_SetCorrectly()
    {
        // Arrange
        // Act
        var testSong = MockSong.MakeSongFileManager();

        // Assert
        Assert.Equal(MockSong.Name, testSong.Name);
        Assert.Equal(MockSong.DataDir, testSong.DataFolder);
        Assert.Equal(MockSong.SongDir, testSong.MusicFolder);
    }

    [Fact]
    public void Constructor_Difficulties_FoundAll()
    {
        // Arrange
        var expectedDiffs = new List<string>
        {
            MockSong.AddDifficulty("test1", new JsonChart()).FullName,
            MockSong.AddDifficulty("test2", new JsonChart()).FullName,
            MockSong.AddDifficulty("", new JsonChart()).FullName
        };

        // Act
        var testSong = MockSong.MakeSongFileManager();
        var testDiffs = testSong.DifficultyFiles.Select(f => f.FullName);

        // Assert
        Assert.Equal(expectedDiffs, testDiffs);
    }

    [Fact]
    public void Constructor_Events_Found()
    {
        // Arrange
        var expectedPath = MockSong.AddEvents(new JsonChart()).FullName;

        // Act
        var testSong = MockSong.MakeSongFileManager();

        // Assert
        Assert.NotNull(testSong.EventsFile);
        Assert.Equal(expectedPath, testSong.EventsFile!.FullName);
    }

    [Fact]
    public void Constructor_SongFiles_Found()
    {
        // Arrange
        var expectedInstPath = MockSong.AddInst().FullName;
        var expectedVoicesPath = MockSong.AddVoices().FullName;

        // Act
        var testSong = MockSong.MakeSongFileManager();

        // Assert
        Assert.Equal(expectedInstPath, testSong.InstFile.FullName);
        Assert.Equal(expectedVoicesPath, testSong.VoicesFile.FullName);
    }

    [Fact]
    public void Constructor_UtilityFile_Created()
    {
        // Arrange
        // Act
        var testSong = MockSong.MakeSongFileManager();
        var mockUtilityFile = MockSong.DataDir.SubDirectory(UtilDirName);

        // Assert
        Assert.True(mockUtilityFile.Exists);
    }

    [Fact]
    public void Constructor_ModificationData_Created()
    {
        // Arrange
        // Act
        MockSong.MakeSongFileManager();
        var mockModificationData = MockSong.DataDir
            .SubDirectory(UtilDirName)
            .File("modification-data.json");

        // Assert
        Assert.True(mockModificationData.Exists);
    }

    [Fact]
    public void Constructor_Data_CreatedBackup()
    {
        // Arrange
        MockSong.AddDifficulty("test", new JsonChart());
        MockSong.AddEvents(new JsonChart());

        // Act
        var testSong = MockSong.MakeSongFileManager();

        var backupDir = MockSong.DataDir
            .SubDirectory(UtilDirName)
            .SubDirectory(BackupDataDirName);

        var backupDifficulty = backupDir.File($"{MockSong.Name}-test.json");
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
        MockSong.AddInst();
        MockSong.AddVoices();

        // Act
        var testSong = MockSong.MakeSongFileManager();

        var backupDir = MockSong.DataDir
            .SubDirectory(UtilDirName)
            .SubDirectory(BackupSongDirName);

        var backupInst = backupDir.File("Inst.ogg");
        var backupVoices = backupDir.File("Voices.ogg");

        // Assert
        Assert.True(backupDir.Exists);
        Assert.True(backupInst.Exists);
        Assert.True(backupVoices.Exists);
    }
}
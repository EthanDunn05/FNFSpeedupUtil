using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using FNFSpeedupUtil.Extensions;
using FNFSpeedupUtil.JsonData;
using FNFSpeedupUtil.JsonData.ChartData;
using FNFSpeedupUtil.SongManagement;
using FNFSpeedupUtil.Tests.AutoData;
using FNFSpeedupUtil.Tests.Mocks;
using Newtonsoft.Json;

namespace FNFSpeedupUtil.Tests.Tests.SongManagement;

public class SongTest
{
    [Theory, AutoJsonChartData]
    public void LoadDifficulties_TwoCharts_LoadedBoth(JsonChart expectedChart)
    {
        // Arrange
        var fs = new MockFileSystem();
        var (data, songs) = CreateFakeSongFolders(fs);

        var expectedChartData = JsonConvert.SerializeObject(expectedChart);
        var testChart1 = data.File("testSong.json");
        testChart1.SerializeJson(expectedChart);
        var testChart2 = data.File("testSong-second.json");
        testChart2.SerializeJson(expectedChart);

        var mockSongFiles = new MockSongFiles("load-the-diffs")
        {
            DifficultyFiles = new List<IFileInfo> { testChart1, testChart2 }
        };

        var song = new Song(mockSongFiles);

        // Act
        var loadedDiffs = song.LoadDifficulties();
        var loadedChartData1 = JsonConvert.SerializeObject(loadedDiffs[testChart1.Name]);
        var loadedChartData2 = JsonConvert.SerializeObject(loadedDiffs[testChart2.Name]);

        // Assure
        Assert.True(loadedDiffs.ContainsKey(testChart1.Name));
        Assert.Equal(expectedChartData, loadedChartData1);
        Assert.Equal(expectedChartData, loadedChartData2);
    }

    [Theory, AutoJsonChartData]
    public void SaveDifficulties_Chart_Saved(JsonChart expectedChart)
    {
        // Arrange
        var fs = new MockFileSystem();
        var (data, songs) = CreateFakeSongFolders(fs);

        var expectedChartData = JsonConvert.SerializeObject(expectedChart);
        var testChart = data.File("testChart.json");

        var mockSongFiles = new MockSongFiles("save-the-diffs")
        {
            DifficultyFiles = new List<IFileInfo> { testChart }
        };
        var song = new Song(mockSongFiles);

        // Act
        song.SaveDifficulty(testChart.Name, expectedChart);
        var savedChartData = fs.File.ReadAllText(testChart.FullName);

        // Assert
        Assert.Equal(expectedChartData, savedChartData);
    }

    [Theory, AutoJsonChartData]
    public void LoadEvents_Chart_Loaded(JsonChart expectedChart)
    {
        // Arrange
        var fs = new MockFileSystem();
        var (data, songs) = CreateFakeSongFolders(fs);

        var expectedChartData = JsonConvert.SerializeObject(expectedChart);
        var events = data.File("events.json");
        events.SerializeJson(expectedChart);
        events.Refresh();

        var mockSongFiles = new MockSongFiles("load-the-events")
        {
            EventsFile = events
        };
        var testSong = new Song(mockSongFiles);

        // Act
        var loadedEvents = testSong.LoadEvents();
        var loadedChartData = JsonConvert.SerializeObject(loadedEvents);

        // Assert
        Assert.Equal(expectedChartData, loadedChartData);
    }

    [Fact]
    public void LoadEvents_NoChart_ThrowsFileNotFoundException()
    {
        // Arrange
        var fs = new MockFileSystem();

        var mockSongFiles = new MockSongFiles("eventless")
        {
            EventsFile = new MockFileInfo(fs, "path to nowhere")
        };
        var testSong = new Song(mockSongFiles);

        // Assert
        Assert.Throws<FileNotFoundException>(() => testSong.LoadEvents());
    }

    [Theory, AutoJsonChartData]
    public void SaveEvents_Chart_Saved(JsonChart expectedChart)
    {
        // Arrange
        var expectedChartData = JsonConvert.SerializeObject(expectedChart);
        
        var fs = new MockFileSystem();
        var (data, songs) = CreateFakeSongFolders(fs);
        
        var events = data.File("events.json");
        events.Create().Close();
        events.Refresh();
        
        var songFiles = new MockSongFiles("save-the-events")
        {
            EventsFile = events
        };
        var song = new Song(songFiles);
        
        // Act
        song.SaveEvents(expectedChart);
        var savedChartData = fs.File.ReadAllText(events.FullName);

        // Assert
        Assert.Equal(expectedChartData, savedChartData);
    }

    [Fact]
    public void SaveEvents_NoChart_ThrowsFileNotFoundException()
    {
        // Arrange
        var fs = new MockFileSystem();

        var mockSongFiles = new MockSongFiles("eventless")
        {
            EventsFile = new MockFileInfo(fs, "but nobody came...")
        };
        var song = new Song(mockSongFiles);
        
        // Assert
        Assert.Throws<FileNotFoundException>(() => song.SaveEvents(new JsonChart()));
    }

    [Fact]
    public void LoadModificationData_File_LoadedCorrectly()
    {
        // Arrange
        var fs = new MockFileSystem();
        var modData = new MockFileInfo(fs, "modData.json");
        var expectedData = new ModificationData();
        modData.SerializeJson(expectedData);
        var expectedText = JsonConvert.SerializeObject(expectedData);

        var mockSongFiles = new MockSongFiles("testSong")
        {
            ModificationDataFile = modData
        };

        // Act
        var testSong = new Song(mockSongFiles);
        var loadedModData = JsonConvert.SerializeObject(testSong.LoadModificationData());

        // Assert
        Assert.Equal(expectedText, loadedModData);
    }

    [Fact]
    public void SaveModificationData_DefaultData_SavesCorrectly()
    {
        // Arrange
        var fs = new MockFileSystem();
        var modData = new MockFileInfo(fs, "modData.json");
        var expectedData = new ModificationData();
        var expectedText = JsonConvert.SerializeObject(expectedData);

        var mockSongFiles = new MockSongFiles("testSong")
        {
            ModificationDataFile = modData
        };
        
        // Act
        var testSong = new Song(mockSongFiles);
        testSong.SaveModificationData(expectedData);
        var writtenText = fs.File.ReadAllText(modData.FullName);
        
        // Assert
        Assert.Equal(expectedText, writtenText);
    }

    [Fact]
    public void LoadBackup_BackupData_LoadedCorrectly()
    {
        // Arrange
        var fs = new MockFileSystem();
        var mockSongFiles = CreateFakeBackupFolders(fs);

        var testData = mockSongFiles.BackupDataFolder.File("testData");
        testData.Create().Close();

        var expectedFile = mockSongFiles.DataFolder.File(testData.Name);
        
        // Act
        var testSong = new Song(mockSongFiles);
        testSong.LoadBackup();
        expectedFile.Refresh();
        
        // Assert
        Assert.True(expectedFile.Exists);
    }

    [Fact]
    public void LoadBackup_BackupSong_LoadedCorrectly()
    {
        // Arrange
        var fs = new MockFileSystem();
        var mockSongFiles = CreateFakeBackupFolders(fs);

        var testSong = mockSongFiles.BackupSongFolder.File("testSong");
        testSong.Create().Close();

        var expectedFile = mockSongFiles.MusicFolder.File(testSong.Name);
        
        // Act
        var song = new Song(mockSongFiles);
        song.LoadBackup();
        expectedFile.Refresh();
        
        // Assert
        Assert.True(expectedFile.Exists);
    }

    [Fact]
    public void LoadBackup_ModificationData_Reset()
    {
        // Ararnge
        var fs = new MockFileSystem();
        var mockSongFiles = CreateFakeBackupFolders(fs);

        var expectedModData = JsonConvert.SerializeObject(new ModificationData());
        
        // Act
        var testSong = new Song(mockSongFiles);
        testSong.LoadBackup();
        var testModData = fs.File.ReadAllText(mockSongFiles.ModificationDataFile.FullName);
        
        // Assert
        Assert.Equal(expectedModData, testModData);
    }

    private (IDirectoryInfo data, IDirectoryInfo songs) CreateFakeSongFolders(IMockFileDataAccessor fs)
    {
        var data = new MockDirectoryInfo(fs, "data");
        data.Create();

        var songs = new MockDirectoryInfo(fs, "songs");
        songs.Create();

        return (data, songs);
    }

    private ISongFiles CreateFakeBackupFolders(IMockFileDataAccessor fs)
    {
        var (data, songs) = CreateFakeSongFolders(fs);

        var backupSongs = data.CreateSubdirectory("backupSongs");
        var backupFolder = data.CreateSubdirectory("backupData");
        var modData = new MockFileInfo(fs, "modData");

        var mockSongFiles = new MockSongFiles("testSong")
        {
            BackupDataFolder = backupFolder,
            BackupSongFolder = backupSongs,
            DataFolder = data,
            MusicFolder = songs,
            ModificationDataFile = modData,
        };

        return mockSongFiles;
    }
}
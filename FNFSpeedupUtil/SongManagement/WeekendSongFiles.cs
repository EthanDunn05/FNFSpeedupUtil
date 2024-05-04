using System.IO.Abstractions;
using FNFSpeedupUtil.Extensions;
using FNFSpeedupUtil.JsonData;
using FNFSpeedupUtil.JsonData.WeekendChartData;
using FNFSpeedupUtil.JsonData.WeekendChartData.Metadata;
using FNFSpeedupUtil.Modifier;
using FNFSpeedupUtil.Modifier.Weekend;

namespace FNFSpeedupUtil.SongManagement;

public class WeekendSongFiles : ISongFiles
{
    public string Name { get; }
    public IDirectoryInfo DataFolder { get; }
    public IDirectoryInfo MusicFolder { get; }
    public IDirectoryInfo UtilityDataFolder { get; }
    public IDirectoryInfo BackupDataFolder { get; }
    public IDirectoryInfo BackupSongFolder { get; }
    public IFileInfo ModificationDataFile { get; }
    
    /// <summary>
    /// The file which holds the chart data
    /// </summary>
    public IFileInfo[] ChartFiles { get; }
    
    /// <summary>
    /// The file which holds the metadata
    /// </summary>
    public IFileInfo[] MetadataFiles { get; }
    
    /// <summary>
    /// All of the music files
    /// </summary>
    public IFileInfo[] SongFiles { get; }

    public WeekendSongFiles(string name, IDirectoryInfo dataFolder, IDirectoryInfo musicFolder)
    {
        Name = name;
        DataFolder = dataFolder;
        MusicFolder = musicFolder;

        // Data files
        var dataFiles = DataFolder.GetFiles();
        ChartFiles = dataFiles.Where(f => f.Name.Contains("chart")).ToArray();
        MetadataFiles = dataFiles.Where(f => f.Name.Contains("metadata")).ToArray();
        
        // Music Files
        SongFiles = MusicFolder.GetFiles();
        
        // Initialize utility folder
        UtilityDataFolder = DataFolder.SubDirectory("SpeedupUtilFiles");
        if (!UtilityDataFolder.Exists) UtilityDataFolder.Create();
        
        // Initialize backup files
        BackupDataFolder = UtilityDataFolder.SubDirectory("backupData");
        BackupSongFolder = UtilityDataFolder.SubDirectory("backupSong");
        if (!BackupDataFolder.Exists || !BackupSongFolder.Exists)
        {
            DataFolder.CopyTo(BackupDataFolder, false);
            MusicFolder.CopyTo(BackupSongFolder, false);
        }
        
        // Initialize modification data
        ModificationDataFile = UtilityDataFolder.File("modification-data.json");
        if (!ModificationDataFile.Exists) ModificationDataFile.SerializeJson(new ModificationData());
        try
        {
            ModificationDataFile.DeserializeJson<ModificationData>();
        }
        catch
        {
            // Default if deserialize fails
            ModificationDataFile.SerializeJson(new ModificationData());
        }
    }

    public async Task ModifySongSpeed(double speed, bool changePitch)
    {
        foreach (var file in ChartFiles)
        {
            var chart = file.DeserializeJson<WeekendJsonChart>();
            WeekendChartModifier.ModifyChartSpeed(chart, speed);
            file.SerializeJson(chart);
        }
        
        foreach(var file in MetadataFiles)
        {
            var metadata = file.DeserializeJson<WeekendJsonMetadata>();
            WeekendChartModifier.ModifyMetadata(metadata, speed);
            file.SerializeJson(metadata);
        }

        // Perocess music modification in parallel
        var musicTasks = new Task[SongFiles.Length];
        for (var i = 0; i < musicTasks.Length; i++)
        {
            var musicFile = SongFiles[i];
            var task = MusicModifier.Modify(musicFile, speed, changePitch);
            musicTasks[i] = task;
        }

        await Task.WhenAll(musicTasks);
        
        // Save modification data
        var modData = LoadModData();
        modData.SpeedModifier *= speed;
        SaveModData(modData);
    }

    public void ModifyScrollSpeed(double scrollSpeed)
    {
        foreach (var file in ChartFiles)
        {
            var chart = file.DeserializeJson<WeekendJsonChart>();
            foreach (var entry in chart.ScrollSpeeds.ScrollSpeeds)
            {
                chart.ScrollSpeeds.ScrollSpeeds[entry.Key] = scrollSpeed;
            }
            file.SerializeJson(chart);
        }
    }

    public void SaveModData(ModificationData data)
    {
        ModificationDataFile.SerializeJson(data);
    }

    public ModificationData LoadModData()
    {
        return ModificationDataFile.DeserializeJson<ModificationData>();
    }

    public void SaveBackup()
    {
        DataFolder.CopyTo(BackupDataFolder, false);
        MusicFolder.CopyTo(BackupSongFolder, false);
    }
    
    public void LoadBackup()
    {
        BackupDataFolder.CopyTo(DataFolder, false);
        BackupSongFolder.CopyTo(MusicFolder, false);

        // Reset the modification file because the data should be reset
        ModificationDataFile.SerializeJson(new ModificationData());
    }
}
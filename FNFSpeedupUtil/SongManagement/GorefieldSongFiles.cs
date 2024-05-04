using System.IO.Abstractions;
using FNFSpeedupUtil.Extensions;
using FNFSpeedupUtil.JsonData;
using FNFSpeedupUtil.JsonData.GorefieldChartData;
using FNFSpeedupUtil.Modifier;

namespace FNFSpeedupUtil.SongManagement;

public class GorefieldSongFiles : ISongFiles
{
    public string Name { get; }
    public IDirectoryInfo DataFolder { get; }
    public IDirectoryInfo UtilityDataFolder { get; }
    public IDirectoryInfo ChartFolder { get; }
    public IDirectoryInfo SongFolder { get; }
    public IFileInfo MetaFile { get; }
    public IDirectoryInfo BackupChartFolder { get; }
    public IDirectoryInfo BackupSongFolder { get; }
    public IFileInfo BackupMetaFile { get; }
    public IFileInfo ModificationDataFile { get; }

    public GorefieldSongFiles(string name, IDirectoryInfo dataFolder)
    {
        Name = name;
        DataFolder = dataFolder;

        ChartFolder = DataFolder.SubDirectory("charts");
        SongFolder = DataFolder.SubDirectory("song");
        MetaFile = DataFolder.File("meta.json");
        
        // Initialize utility folder
        UtilityDataFolder = DataFolder.SubDirectory("SpeedupUtilFiles");
        if (!UtilityDataFolder.Exists) UtilityDataFolder.Create();
        
        // Initialize backup files
        BackupChartFolder = UtilityDataFolder.SubDirectory("backupChart");
        BackupSongFolder = UtilityDataFolder.SubDirectory("backupSong");
        BackupMetaFile = UtilityDataFolder.File("meta.json");
        if (!BackupChartFolder.Exists || !BackupSongFolder.Exists || !BackupMetaFile.Exists)
        {
            ChartFolder.CopyTo(BackupChartFolder, false);
            SongFolder.CopyTo(BackupSongFolder, false);
            MetaFile.CopyTo(BackupMetaFile.FullName, true);
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
        foreach (var chartFile in ChartFolder.GetFiles())
        {
            var chart = chartFile.DeserializeJson<GorefieldJsonChart>();
            GorefieldChartModifier.ModifyChartSpeed(chart, speed);
            chartFile.SerializeJson(chart);
        }

        var metadata = MetaFile.DeserializeJson<GorefieldJsonMetadata>();
        GorefieldChartModifier.ModifyMetadata(metadata, speed);
        MetaFile.SerializeJson(metadata);

        var songFiles = SongFolder.GetFiles();
        var musicTasks = new Task[songFiles.Length];
        for (var i = 0; i < songFiles.Length; i++)
        {
            var file = songFiles[i];
            musicTasks[i] = MusicModifier.Modify(file, speed, changePitch);
        }

        await Task.WhenAll(musicTasks);
        
        // Save modification data
        var modData = LoadModData();
        modData.SpeedModifier *= speed;
        SaveModData(modData);
    }

    public void ModifyScrollSpeed(double scrollSpeed)
    {
        foreach (var chartFile in ChartFolder.GetFiles())
        {
            var chart = chartFile.DeserializeJson<GorefieldJsonChart>();
            chart.ScrollSpeed = scrollSpeed;
            chartFile.SerializeJson(chart);
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
        ChartFolder.CopyTo(BackupChartFolder, false);
        SongFolder.CopyTo(BackupSongFolder, false);
        MetaFile.CopyTo(BackupMetaFile.FullName, true);
    }

    public void LoadBackup()
    {
        BackupChartFolder.CopyTo(ChartFolder, false);
        BackupSongFolder.CopyTo(SongFolder, false);
        BackupMetaFile.CopyTo(MetaFile.FullName, true);
        
        // Reset the modification file because the data should be reset
        ModificationDataFile.SerializeJson(new ModificationData());
    }
}
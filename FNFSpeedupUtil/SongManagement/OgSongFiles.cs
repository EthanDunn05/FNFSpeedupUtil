using System.IO.Abstractions;
using FNFSpeedupUtil.Extensions;
using FNFSpeedupUtil.JsonData;
using FNFSpeedupUtil.JsonData.ChartData;
using FNFSpeedupUtil.Modifier;

namespace FNFSpeedupUtil.SongManagement;

/// <summary>
/// Holds links to all the song files. Initializes data files when created.
/// </summary>
public class OgSongFiles : ISongFiles
{
    /// <inheritdoc />
    public string Name { get; }
    
    /// <inheritdoc />
    public IDirectoryInfo DataFolder { get; }
    
    /// <inheritdoc />
    public IDirectoryInfo MusicFolder { get; }
    
    /// <inheritdoc />
    public IFileInfo InstFile { get; }
    
    /// <inheritdoc />
    public IFileInfo VoicesFile { get; }
    
    /// <inheritdoc />
    public List<IFileInfo> DifficultyFiles { get; }
    
    /// <inheritdoc />
    public IFileInfo EventsFile { get; }
    
    /// <inheritdoc />
    public IDirectoryInfo UtilityDataFolder { get; }
    
    /// <inheritdoc />
    public IDirectoryInfo BackupDataFolder { get; }
    
    /// <inheritdoc />
    public IDirectoryInfo BackupSongFolder { get; }
    
    /// <inheritdoc />
    public IFileInfo ModificationDataFile { get; }

    public async Task ModifySongSpeed(double speed, bool changePitch)
    {
        // Load the chart and track them with their file
        var charts = new Dictionary<IFileInfo, OgJsonChart>();
        
        // Load difficulty files
        foreach (var difficultyFile in DifficultyFiles)
        {
            var diff = difficultyFile.DeserializeJson<OgJsonChart>();
                charts.Add(difficultyFile, diff);
        }

        // Load events file
        if (EventsFile.Exists)
        {
            charts.Add(EventsFile, EventsFile.DeserializeJson<OgJsonChart>());
        }

        // Modify the files
        foreach (var (file, chart) in charts)
        {
            OgChartModifier.ModifySpeed(chart, speed);
            file.SerializeJson(chart);
        }
        
        var modifyInstTask = Task.CompletedTask;
        if (InstFile.Exists)
        {
            // Create the modification task
            modifyInstTask = MusicModifier.Modify(InstFile, speed, changePitch);
        }

        // Make a task to modify the voices if it exists
        var modifyVoicesTask = Task.CompletedTask;
        if (VoicesFile.Exists)
        {
            // Create the modification task
            modifyVoicesTask = MusicModifier.Modify(VoicesFile, speed, changePitch);
        }

        // Wait all for processing the files in parallel
        await Task.WhenAll(modifyInstTask, modifyVoicesTask);

        // Save modification data
        var modData = LoadModData();
        modData.SpeedModifier *= speed;
        SaveModData(modData);
    }

    public void ModifyScrollSpeed(double scrollSpeed)
    {
        foreach (var difficultyFile in DifficultyFiles)
        {
            var diff = difficultyFile.DeserializeJson<OgJsonChart>();
            diff.Song.Speed = scrollSpeed;
            difficultyFile.SerializeJson(diff);
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="dataFolder"></param>
    /// <param name="musicFolder"></param>
    public OgSongFiles(string name, IDirectoryInfo dataFolder, IDirectoryInfo musicFolder)
    {
        Name = name;
        DataFolder = dataFolder;
        MusicFolder = musicFolder;

        // Initialize chart files
        var dataFiles = dataFolder.GetFiles();
        DifficultyFiles = dataFiles.Where(IsChartFile).ToList();
        EventsFile = dataFolder.File("events.json");

        // Initialize music files
        InstFile = MusicFolder.File("Inst.ogg");
        VoicesFile = MusicFolder.File("Voices.ogg");

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
    
    /// <summary>
    /// Test if a path leads to a chart file
    /// </summary>
    /// <param name="file">Path to the file to test</param>
    /// <returns>Weather or not a file is a chart file</returns>
    private bool IsChartFile(IFileInfo file)
    {
        var followsNamingScheme = file.Name.StartsWith(Name);
        var isJsonFile = file.Extension == ".json";
        return followsNamingScheme && isJsonFile;
    }
}
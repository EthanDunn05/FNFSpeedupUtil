using System.IO.Abstractions;
using FNFSpeedupUtil.Extensions;
using FNFSpeedupUtil.JsonData;

namespace FNFSpeedupUtil.SongManagement;

/// <summary>
/// Holds links to all the song files. Initializes data files when created.
/// </summary>
public class SongFiles : ISongFiles
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="dataFolder"></param>
    /// <param name="musicFolder"></param>
    public SongFiles(string name, IDirectoryInfo dataFolder, IDirectoryInfo musicFolder)
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
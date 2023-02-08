using System.IO.Abstractions;
using FNFSpeedupUtil.Extensions;
using FNFSpeedupUtil.JsonData;

namespace FNFSpeedupUtil.SongManagement;

/// <summary>
/// Controls the symbolic links and initialization of files
/// </summary>
public class SongFiles
{
    /// <summary>
    /// The song name (unformatted).
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The song's data folder. Holds the chart files and the events file.
    /// </summary>
    public IDirectoryInfo DataFolder { get; }

    /// <summary>
    /// The song's song folder. Holds the music files.
    /// </summary>
    public IDirectoryInfo MusicFolder { get; }

    /// <summary>
    /// The song instrumental file.
    /// </summary>
    public IFileInfo InstFile { get; }

    /// <summary>
    /// The song voices file.
    /// </summary>
    public IFileInfo VoicesFile { get; }

    /// <summary>
    /// A list of the difficulty chart files that the song has.
    /// </summary>
    public List<IFileInfo> DifficultyFiles { get; }

    /// <summary>
    /// The events chart file.
    /// </summary>
    public IFileInfo EventsFile { get; }

    /// <summary>
    /// The directory which holds all of the files created and managed by this
    /// </summary>
    public IDirectoryInfo UtilityDataFolder { get; }

    /// <summary>
    /// The directory in which backup data is saved.
    /// </summary>
    public IDirectoryInfo BackupDataFolder { get; }
    
    /// <summary>
    /// The directory in which backup songs are saved.
    /// </summary>
    public IDirectoryInfo BackupSongFolder { get; }

    /// <summary>
    /// The modification data file.
    /// </summary>
    public IFileInfo ModificationDataFile { get; }

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
        if (!ModificationDataFile.Exists)
        {
            ModificationDataFile.Create();
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
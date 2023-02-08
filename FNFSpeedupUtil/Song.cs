using System.IO.Abstractions;
using FNFSpeedupUtil.Extensions;
using FNFSpeedupUtil.Helpers;
using FNFSpeedupUtil.JsonData;
using Newtonsoft.Json;

namespace FNFSpeedupUtil;

/// <summary>
/// Has all of the information of the song files.
/// </summary>
public class Song
{
    /// <summary>
    /// The song name (unformatted).
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The path to the song's data folder. Holds the chart
    /// files and the events file.
    /// </summary>
    public IDirectoryInfo DataDir { get; }

    /// <summary>
    /// The path to the song's song folder. Holds the music file.
    /// </summary>
    public IDirectoryInfo SongDir { get; }

    public bool HasBackup => BackupDataDir.Exists;

    /// <summary>
    /// Path to the instrumental file.
    /// </summary>
    public IFileInfo InstFile { get; }

    /// <summary>
    /// Path to the voices file.
    /// </summary>
    public IFileInfo VoicesFile { get; }

    /// <summary>
    /// A list of the paths to all the difficulties that the song has.
    /// </summary>
    public List<IFileInfo> DifficultyFiles { get; }

    /// <summary>
    /// The path to the events file. Can be null.
    /// </summary>
    public IFileInfo? EventsFile { get; }

    public ModificationData ModificationData { get; }

    public IDirectoryInfo UtilityDataDir { get; }

    public IDirectoryInfo BackupDataDir { get; }
    public IDirectoryInfo BackupSongDir { get; }

    public IFileInfo ModificationDataFile { get; }

    public Song(string name, IDirectoryInfo dataDir, IDirectoryInfo songDir)
    {
        Name = name;
        DataDir = dataDir;
        SongDir = songDir;

        // Get all of the files and directories
        var dataFiles = dataDir.GetFiles();
        DifficultyFiles = dataFiles.Where(IsChartFile).ToList();

        EventsFile = dataFiles.FirstOrDefault(path => path.Name == "events.json");

        InstFile = SongDir.File("Inst.ogg");
        VoicesFile = SongDir.File("Voices.ogg");

        UtilityDataDir = DataDir.SubDirectory("SpeedupUtilFiles");
        BackupDataDir = UtilityDataDir.SubDirectory("backupData");
        BackupSongDir = UtilityDataDir.SubDirectory("backupSong");

        ModificationDataFile = UtilityDataDir.File("modification-data.json");

        // Create the utility folder if it doesn't exist
        if (!UtilityDataDir.Exists) UtilityDataDir.Create();

        ModificationData = ReadOrCreateModificationData();
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

    private ModificationData ReadOrCreateModificationData()
    {
        if (ModificationDataFile.Exists)
            return ModificationDataFile.DeserializeJson<ModificationData>();

        ModificationDataFile.Create();
        var newData = new ModificationData();
        ModificationDataFile.SerializeJson(newData);
        return newData;
    }

    /// <summary>
    /// Makes a backup of the data and song folders. Stores the
    /// backups in a subfolder of themselves called backup.
    /// </summary>
    public void MakeBackup()
    {
        DataDir.CopyTo(BackupDataDir, false);
        SongDir.CopyTo(BackupSongDir, false);
    }

    /// <summary>
    /// Loads a backup of the data and song folders. Loads the
    /// backups from a subfolder of themselves called backup.
    /// </summary>
    public void LoadBackup()
    {
        BackupDataDir.CopyTo(DataDir, false);
        BackupSongDir.CopyTo(SongDir, false);

        // Reset the modification file because the data should be reset
        var newData = new ModificationData();
        ModificationDataFile.SerializeJson(newData);
    }
}
using FNFSpeedupUtil.Helpers;

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
    public string DataPath { get; }

    /// <summary>
    /// The path to the song's song folder. Holds the music file.
    /// </summary>
    public string SongPath { get; }

    public bool HasBackup => Directory.Exists(BackupDir);

    /// <summary>
    /// Path to the instrumental file.
    /// </summary>
    public string InstPath => Path.Join(SongPath, @"/Inst.ogg");

    /// <summary>
    /// Path to the voices file.
    /// </summary>
    public string VoicesPath => Path.Join(SongPath, @"/Voices.ogg");

    /// <summary>
    /// A list of the paths to all the difficulties that the song has.
    /// </summary>
    public List<string> DifficultyPaths { get; }

    /// <summary>
    /// The path to the events file. Can be null.
    /// </summary>
    public string? EventsPath { get; }

    public ModificationData ModificationData { get; private set; }

    private string UtilityDataDir => Path.Join(DataPath, @"/SpeedupUtilFiles/");
    
    private string BackupDir => Path.Join(UtilityDataDir, @"/backup/");
    
    private string ModificationDataPath => Path.Join(UtilityDataDir, @"/modification-data.json");

    public Song(string name, string dataPath, string songPath)
    {
        Name = name;
        DataPath = dataPath;
        SongPath = songPath;

        // Find the difficulties
        var dataFiles = Directory.GetFiles(dataPath);
        DifficultyPaths = dataFiles.Where(IsChartFile).ToList();

        // Find the events
        EventsPath = dataFiles.FirstOrDefault(path => Path.GetFileName(path) == "events.json");

        // Create the utility folder if it doesn't exist
        if (!Directory.Exists(UtilityDataDir)) Directory.CreateDirectory(UtilityDataDir);

        // Read the modification data if it exists, or make a new one
        if (File.Exists(ModificationDataPath))
            ModificationData = ModificationData.Deserialize(ModificationDataPath);
        else ResetModificationFile();
    }

    /// <summary>
    /// Test if a path leads to a chart file
    /// </summary>
    /// <param name="path">Path to the file to test</param>
    /// <returns>Weather or not a file is a chart file</returns>
    private bool IsChartFile(string path)
    {
        var followsNamingScheme = Path.GetFileName(path).StartsWith(Name);
        var isJsonFile = Path.GetExtension(path) == ".json";
        return followsNamingScheme && isJsonFile;
    }

    /// <summary>
    /// Resets the modification file. Effectively writes all properties to default.
    /// </summary>
    private void ResetModificationFile()
    {
        ModificationData = new ModificationData();
        ModificationData.Serialize(ModificationDataPath);
    }

    /// <summary>
    /// Re-serializes the modification file.
    /// </summary>
    public void UpdateModificationFile()
    {
        ModificationData.Serialize(ModificationDataPath);
    }

    /// <summary>
    /// Makes a backup of the data and song folders. Stores the
    /// backups in a subfolder of themselves called backup.
    /// </summary>
    public void MakeBackup()
    {
        if (!Directory.Exists(BackupDir)) Directory.CreateDirectory(BackupDir);

        // Save difficulties
        foreach (var diff in DifficultyPaths)
        {
            var backupFilePath = Path.Join(BackupDir, Path.GetFileName(diff));
            File.Copy(diff, backupFilePath, true);
        }

        // Save music
        File.Copy(Path.Join(SongPath, "Inst.ogg"), Path.Join(BackupDir, "Inst.ogg"), true);
        
        if(File.Exists(VoicesPath))
            File.Copy(Path.Join(SongPath, "Voices.ogg"), Path.Join(BackupDir, "Voices.ogg"), true);
    }

    /// <summary>
    /// Loads a backup of the data and song folders. Loads the
    /// backups from a subfolder of themselves called backup.
    /// </summary>
    public void LoadBackup()
    {
        try
        {
            // Load difficulties
            foreach (var diff in DifficultyPaths)
            {
                var backup = Path.Join(BackupDir, Path.GetFileName(diff));
                File.Copy(backup, diff, true);
            }

            // Load music
            File.Copy(Path.Join(BackupDir, "Inst.ogg"), Path.Join(SongPath, "Inst.ogg"), true);
            File.Copy(Path.Join(BackupDir, "Voices.ogg"), Path.Join(SongPath, "Voices.ogg"), true);
        }
        catch (FileNotFoundException e)
        {
            // Catch when the backup doesn't exist
            // Just Ignore it :P
        }

        // Reset the modification file because the data should be reset
        ResetModificationFile();
    }
}
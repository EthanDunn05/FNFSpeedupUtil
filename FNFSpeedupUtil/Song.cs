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
    /// Makes a backup of the data and song folders. Stores the
    /// backups in a subfolder of themselves called backup.
    /// </summary>
    public void MakeBackup()
    {
        var dataBackup = Path.Join(DataPath, @"/backup");
        var songBackup = Path.Join(SongPath, @"/backup");
        
        if (Directory.Exists(songBackup)) return;
        if (Directory.Exists(dataBackup)) return;
        
        DirectoryHelper.CopyDirectory(DataPath, dataBackup, false);
        DirectoryHelper.CopyDirectory(SongPath, songBackup, false);
    }

    /// <summary>
    /// Loads a backup of the data and song folders. Loads the
    /// backups from a subfolder of themselves called backup.
    /// </summary>
    public void LoadBackup()
    {
        var dataBackup = Path.Join(DataPath, @"/backup");
        var songBackup = Path.Join(SongPath, @"/backup");
        
        if (!Directory.Exists(songBackup)) return;
        if (!Directory.Exists(dataBackup)) return;
        
        DirectoryHelper.CopyDirectory(dataBackup, DataPath, false);
        DirectoryHelper.CopyDirectory(songBackup, SongPath, false);
    }
}
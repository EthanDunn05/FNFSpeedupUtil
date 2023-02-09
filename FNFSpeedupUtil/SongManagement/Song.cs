using System.IO.Abstractions;
using FNFSpeedupUtil.Extensions;
using FNFSpeedupUtil.JsonData;
using FNFSpeedupUtil.JsonData.ChartData;

namespace FNFSpeedupUtil.SongManagement;

/// <summary>
/// Manages saving and loading data to the files
/// </summary>
public class Song : ISong
{
    /// <summary>
    /// Contains all the files needed to do stuff
    /// </summary>
    private ISongFiles Files { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public bool HasEventsChart => Files.EventsFile.Exists;

    /// <inheritdoc />
    public List<string> DifficultyNames => Files.DifficultyFiles.Select(file => file.Name).ToList();

    /// <inheritdoc />
    public IFileInfo InstFile => Files.InstFile;

    /// <inheritdoc />
    public IFileInfo VoicesFile => Files.VoicesFile;

    /// <summary>
    /// Creates a new song representation.
    /// </summary>
    /// <param name="name">The name of this song</param>
    /// <param name="dataDir">The directory of the chart files</param>
    /// <param name="songDir">The directory of the song files</param>
    public Song(string name, IDirectoryInfo dataDir, IDirectoryInfo songDir)
        : this(new SongFiles(name, dataDir, songDir))
    {
    }

    /// <summary>
    /// Creates a new song representation from given song files
    /// </summary>
    /// <param name="files">The files of this song</param>
    public Song(ISongFiles files)
    {
        Name = files.Name;
        Files = files;
    }

    /// <inheritdoc />
    public Dictionary<string, JsonChart> LoadDifficulties()
    {
        var result = new Dictionary<string, JsonChart>();
        foreach (var difficultyFile in Files.DifficultyFiles)
        {
            var name = difficultyFile.Name;
            var chart = difficultyFile.DeserializeJson<JsonChart>();
            result.Add(name, chart);
        }

        return result;
    }

    /// <inheritdoc />
    public void SaveDifficulty(string difficultyName, JsonChart chart)
    {
        var difficultyFile = Files.DifficultyFiles.First(file => file.Name == difficultyName);
        if (difficultyFile == null) throw new FileNotFoundException($"{difficultyName} is not a valid difficulty");

        difficultyFile.SerializeJson(chart);
    }

    /// <inheritdoc />
    public JsonChart LoadEvents()
    {
        if (HasEventsChart) return Files.EventsFile.DeserializeJson<JsonChart>();

        throw new FileNotFoundException("The events file does not exist");
    }

    /// <inheritdoc />
    public void SaveEvents(JsonChart chart)
    {
        if (!HasEventsChart) throw new FileNotFoundException("The events file does not exist");
        Files.EventsFile.SerializeJson(chart);
    }

    /// <inheritdoc />
    public ModificationData LoadModificationData()
    {
        return Files.ModificationDataFile.DeserializeJson<ModificationData>();
    }

    /// <inheritdoc />
    public void SaveModificationData(ModificationData data)
    {
        Files.ModificationDataFile.SerializeJson(data);
    }

    /// <inheritdoc />
    public void LoadBackup()
    {
        Files.BackupDataFolder.CopyTo(Files.DataFolder, false);
        Files.BackupSongFolder.CopyTo(Files.MusicFolder, false);

        // Reset the modification file because the data should be reset
        Files.ModificationDataFile.SerializeJson(new ModificationData());
    }
}
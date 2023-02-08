using System.IO.Abstractions;
using FNFSpeedupUtil.Extensions;
using FNFSpeedupUtil.JsonData;
using FNFSpeedupUtil.JsonData.ChartData;

namespace FNFSpeedupUtil.SongManagement;

/// <summary>
/// Manages the data of a song. Much of the data is managed through a save and load
/// system to simplify managing files.
/// </summary>
public class Song
{
    /// <summary>
    /// Contains all the files needed to do stuff
    /// </summary>
    private SongFiles Files { get; }

    /// <summary>
    /// The name of the song
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Whether or not the song has an events chart
    /// </summary>
    public bool HasEventsChart => Files.EventsFile.Exists;

    /// <summary>
    /// The names of all the difficulty files.
    /// </summary>
    public List<string> DifficultyNames => Files.DifficultyFiles.Select(file => file.Name).ToList();
    
    /// <summary>
    /// Expose the inst file
    /// </summary>
    public IFileInfo InstFile => Files.InstFile;

    /// <summary>
    /// Expose the voices file
    /// </summary>
    public IFileInfo VoicesFile => Files.VoicesFile;

    /// <summary>
    /// Creates a new song representation.
    /// </summary>
    /// <param name="name">The name of this song</param>
    /// <param name="dataDir">The directory of the chart files</param>
    /// <param name="songDir">The directory of the song files</param>
    public Song(string name, IDirectoryInfo dataDir, IDirectoryInfo songDir)
    {
        Name = name;
        Files = new SongFiles(name, dataDir, songDir);
    }
    
    /// <summary>
    /// Loads the difficulty charts.
    /// </summary>
    /// <returns>A dictionary of the difficulty charts, keys are the chart names</returns>
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

    /// <summary>
    /// Saves a chart to a difficulty.
    /// </summary>
    /// <param name="difficultyName">The name of the difficulty to save to</param>
    /// <param name="chart">The chart data to save</param>
    /// <exception cref="ApplicationException">
    /// The difficulty could not be found in the list of difficulties. Check <see cref="DifficultyNames"/>
    /// if you are unsure a difficulty is valid.
    /// </exception>
    public void SaveDifficulty(string difficultyName, JsonChart chart)
    {
        var difficultyFile = Files.DifficultyFiles.First(file => file.Name == difficultyName);
        if (difficultyFile == null) throw new FileNotFoundException($"{difficultyName} is not a valid difficulty");

        difficultyFile.SerializeJson(chart);
    }

    /// <summary>
    /// Loads the events file for the song
    /// </summary>
    /// <returns>The chart data of the file</returns>
    /// <exception cref="FileNotFoundException">
    /// The events file does not exist. Check <see cref="HasEventsChart"/>
    /// before calling this to avoid the exception.
    /// </exception>
    public JsonChart LoadEvents()
    {
        if (HasEventsChart) return Files.EventsFile.DeserializeJson<JsonChart>();

        throw new FileNotFoundException("The events file does not exist");
    }

    /// <summary>
    /// Saves the chart to the events file.
    /// </summary>
    /// <param name="chart">The chart data to save</param>
    /// <exception cref="FileNotFoundException">
    /// The events file does not exist. Check <see cref="HasEventsChart"/>
    /// before calling this to avoid the exception.
    /// </exception>
    public void SaveEvents(JsonChart chart)
    {
        if (HasEventsChart) Files.EventsFile.SerializeJson(chart);

        throw new FileNotFoundException("The events file does not exist");
    }

    /// <summary>
    /// Loads the modification data 
    /// </summary>
    /// <returns>The modification data</returns>
    public ModificationData LoadModificationData()
    {
        return Files.ModificationDataFile.DeserializeJson<ModificationData>();
    }

    /// <summary>
    /// Saves new data to the modification data
    /// </summary>
    /// <param name="data">The data to save</param>
    public void SaveModificationData(ModificationData data)
    {
        Files.ModificationDataFile.SerializeJson(data);
    }

    /// <summary>
    /// Loads a backup of the data and song folders. Loads the
    /// backups from a subfolder of themselves called backup.
    /// </summary>
    public void LoadBackup()
    {
        Files.BackupDataFolder.CopyTo(Files.DataFolder, false);
        Files.BackupSongFolder.CopyTo(Files.MusicFolder, false);

        // Reset the modification file because the data should be reset
        Files.ModificationDataFile.SerializeJson(new ModificationData());
    }
}
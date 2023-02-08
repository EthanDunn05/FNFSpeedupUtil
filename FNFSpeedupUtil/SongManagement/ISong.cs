using System.IO.Abstractions;
using FNFSpeedupUtil.Extensions;
using FNFSpeedupUtil.JsonData;
using FNFSpeedupUtil.JsonData.ChartData;

namespace FNFSpeedupUtil.SongManagement;

/// <summary>
/// Manages the data of a song. Much of the data is managed through a save and load
/// system to simplify managing files.
/// </summary>
public interface ISong
{
    /// <summary>
    /// The name of the song
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Whether or not the song has an events chart
    /// </summary>
    public bool HasEventsChart { get; }

    /// <summary>
    /// The names of all the difficulty files.
    /// </summary>
    public List<string> DifficultyNames { get; }

    /// <summary>
    /// Expose the inst file
    /// </summary>
    public IFileInfo InstFile { get; }

    /// <summary>
    /// Expose the voices file
    /// </summary>
    public IFileInfo VoicesFile { get; }

    /// <summary>
    /// Loads the difficulty charts.
    /// </summary>
    /// <returns>A dictionary of the difficulty charts, keys are the chart names</returns>
    public Dictionary<string, JsonChart> LoadDifficulties();

    /// <summary>
    /// Saves a chart to a difficulty.
    /// </summary>
    /// <param name="difficultyName">The name of the difficulty to save to</param>
    /// <param name="chart">The chart data to save</param>
    /// <exception cref="ApplicationException">
    /// The difficulty could not be found in the list of difficulties. Check <see cref="DifficultyNames"/>
    /// if you are unsure a difficulty is valid.
    /// </exception>
    public void SaveDifficulty(string difficultyName, JsonChart chart);

    /// <summary>
    /// Loads the events file for the song
    /// </summary>
    /// <returns>The chart data of the file</returns>
    /// <exception cref="FileNotFoundException">
    /// The events file does not exist. Check <see cref="HasEventsChart"/>
    /// before calling this to avoid the exception.
    /// </exception>
    public JsonChart LoadEvents();

    /// <summary>
    /// Saves the chart to the events file.
    /// </summary>
    /// <param name="chart">The chart data to save</param>
    /// <exception cref="FileNotFoundException">
    /// The events file does not exist. Check <see cref="HasEventsChart"/>
    /// before calling this to avoid the exception.
    /// </exception>
    public void SaveEvents(JsonChart chart);

    /// <summary>
    /// Loads the modification data 
    /// </summary>
    /// <returns>The modification data</returns>
    public ModificationData LoadModificationData();

    /// <summary>
    /// Saves new data to the modification data
    /// </summary>
    /// <param name="data">The data to save</param>
    public void SaveModificationData(ModificationData data);

    /// <summary>
    /// Loads a backup of the data and song folders. Loads the
    /// backups from a subfolder of themselves called backup.
    /// </summary>
    public void LoadBackup();
}
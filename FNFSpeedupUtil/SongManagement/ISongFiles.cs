using System.IO.Abstractions;
using FNFSpeedupUtil.Extensions;
using FNFSpeedupUtil.JsonData;

namespace FNFSpeedupUtil.SongManagement;

/// <summary>
/// Controls the symbolic links and initialization of files
/// </summary>
public interface ISongFiles
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
}
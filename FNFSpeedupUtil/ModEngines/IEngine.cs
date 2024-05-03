using System.IO.Abstractions;
using FNFSpeedupUtil.SongManagement;

namespace FNFSpeedupUtil.ModEngines;

public interface IEngine
{
    /// <summary>
    /// Finds all of the songs in this mod.
    /// </summary>
    /// <param name="modRoot">The root directory of the mod</param>
    /// <returns>A list of the songs found.</returns>
    public List<ISongFiles> FindSongs(IDirectoryInfo modRoot);

    /// <summary>
    /// Determines if this mod engine is a valid option for the given mod Directory
    /// </summary>
    /// <param name="modRoot">The root directory of the mod</param>
    /// <returns>If the mod fits the requirements for the engine.</returns>
    public bool ValidForMod(IDirectoryInfo modRoot);
}
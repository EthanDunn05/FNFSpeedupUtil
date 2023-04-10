using System.IO.Abstractions;
using FNFSpeedupUtil.SongManagement;

namespace FNFSpeedupUtil.ModEngines;

public class PsychEngine : IEngine
{
    /// <inheritdoc/>
    public List<ISong> FindSongs(IDirectoryInfo modRoot)
    {
        var dataDir = modRoot.SubDirectory("mods").SubDirectory("data");
        var songDataDirs = dataDir.GetDirectories();

        if (songDataDirs.Length <= 0) throw new DirectoryNotFoundException();

        return (from dir in songDataDirs
            let songName = dir.Name
            let songDir = modRoot.SubDirectory("mods").SubDirectory("songs").SubDirectory(songName)
            where songDir.Exists
            select new Song(songName, dir, songDir)).Cast<ISong>().ToList();
    }

    /// <inheritdoc/>
    public bool ValidForMod(IDirectoryInfo modRoot)
    {
        try
        {
            var hasModsData = modRoot.SubDirectory("mods").SubDirectory("data").Exists;
            var hasModsSongs = modRoot.SubDirectory("mods").SubDirectory("songs").Exists;
            var hasSongs = modRoot.SubDirectory("mods").SubDirectory("data").GetDirectories().Length > 0;

            return hasModsData && hasModsSongs && hasSongs;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
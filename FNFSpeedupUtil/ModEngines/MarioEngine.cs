using System.IO.Abstractions;
using FNFSpeedupUtil.SongManagement;

namespace FNFSpeedupUtil.ModEngines;

public class MarioEngine : IEngine
{
    /// <inheritdoc/>
    public List<ISong> FindSongs(IDirectoryInfo modRoot)
    {
        var dataDir = modRoot.SubDirectory("assets").SubDirectory("data").SubDirectory("songData");
        var songDataDirs = dataDir.GetDirectories();

        if (songDataDirs.Length <= 0) throw new DirectoryNotFoundException();

        return (from dir in songDataDirs
            let songName = dir.Name
            let songDir = modRoot.SubDirectory("assets").SubDirectory("songs").SubDirectory(songName)
            where songDir.Exists
            select new Song(songName, dir, songDir)).Cast<ISong>().ToList();
    }

    /// <inheritdoc/>
    public bool ValidForMod(IDirectoryInfo modRoot)
    {
        try
        {
            var hasAssetsSongs = modRoot.SubDirectory("assets").SubDirectory("songs").Exists;
            var hasSongData = modRoot.SubDirectory("assets").SubDirectory("data").SubDirectory("songData").Exists;
            var hasSongs = modRoot.SubDirectory("assets").SubDirectory("songs").GetDirectories().Length > 0;

            return hasAssetsSongs && hasSongData && hasSongs;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
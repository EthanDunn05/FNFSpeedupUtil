using System.IO.Abstractions;
using FNFSpeedupUtil.SongManagement;

namespace FNFSpeedupUtil.ModEngines;

public class HypnoEngine : IEngine
{
    /// <inheritdoc/>
    public List<ISong> FindSongs(IDirectoryInfo modRoot)
    {
        var dataDir = modRoot.SubDirectory("assets").SubDirectory("songs");
        var songDataDirs = dataDir.GetDirectories();

        if (songDataDirs.Length <= 0) throw new DirectoryNotFoundException();

        return (from dir in songDataDirs
            let songName = dir.Name
            select new Song(songName, dir, dir)).Cast<ISong>().ToList();
    }

    /// <inheritdoc/>
    public bool ValidForMod(IDirectoryInfo modRoot)
    {
        try
        {
            var hasAssetsSongs = modRoot.SubDirectory("assets").SubDirectory("songs").Exists;
            var noAssetsData = !modRoot.SubDirectory("assets").SubDirectory("data").Exists;
            var hasSongs = modRoot.SubDirectory("assets").SubDirectory("songs").GetDirectories().Length > 0;

            return hasAssetsSongs && noAssetsData && hasSongs;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
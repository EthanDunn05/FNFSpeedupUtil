using System.IO.Abstractions;
using FNFSpeedupUtil.SongManagement;

namespace FNFSpeedupUtil.ModEngines;

public class GorefieldEngine : IEngine
{
    public List<ISongFiles> FindSongs(IDirectoryInfo modRoot)
    {
        var dataDir = modRoot.SubDirectory("mods").SubDirectory("gorefield").SubDirectory("songs");
        var songDataDirs = dataDir.GetDirectories();

        if (songDataDirs.Length <= 0) throw new DirectoryNotFoundException();

        return (from dir in songDataDirs
            let songName = dir.Name
            select new GorefieldSongFiles(songName, dir)).Cast<ISongFiles>().ToList();
    }

    public bool ValidForMod(IDirectoryInfo modRoot)
    {
        try
        {
            // Simple check
            var gorefieldMod = modRoot.SubDirectory("mods").SubDirectory("gorefield").Exists;
            return gorefieldMod;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
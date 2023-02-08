using System.IO.Abstractions;
using FNFSpeedupUtil.SongManagement;

namespace FNFSpeedupUtil.Helpers;

/// <summary>
/// Helper class for getting input from the user and reading
/// the mod directory
/// </summary>
public static class ModDirectoryHelper
{
    /// <summary>
    /// Finds all of the songs in the given mod.
    /// </summary>
    /// <param name="modDir">The path to the mod folder</param>
    /// <returns>An array of all of the songs in the mod</returns>
    public static List<Song> FindSongs(IDirectoryInfo modDir)
    {
        // Check assets folder
        try
        {
            var dataDir = modDir.SubDirectory("assets").SubDirectory("data");
            var songDataDirs = dataDir.GetDirectories();
            
            return (from songDataDir in songDataDirs
                let songName = songDataDir.Name
                let songPath = modDir.SubDirectory("assets").SubDirectory("songs").SubDirectory(songName)
                select new Song(songName, songDataDir, songPath)).ToList();
        }
        catch (DirectoryNotFoundException e)
        {
            // Do nothing
        }

        // Check mod folder
        try
        {
            var dataDir = modDir.SubDirectory("mods").SubDirectory("data");
            var songDataDirs = dataDir.GetDirectories();

            return (from songDataDir in songDataDirs
                let songName = songDataDir.Name
                let songPath = modDir.SubDirectory("mods").SubDirectory("songs").SubDirectory(songName)
                select new Song(songName, songDataDir, songPath)).ToList();
        }
        catch (DirectoryNotFoundException e)
        {
            // Do nothing
        }

        return new List<Song>();
    }
}
using System.IO.Abstractions;
using FNFSpeedupUtil.Helpers;

namespace FNFSpeedupUtil.Menu.Pages;

public class LoadAllBackups : Page
{
    private IDirectoryInfo ModDir { get; }

    public LoadAllBackups(IDirectoryInfo modDir)
    {
        ModDir = modDir;
    }

    protected override void Render()
    {
        var songs = ModDirectoryHelper.FindSongs(ModDir);

        foreach (var song in songs)
        {
            try
            {
                song.LoadBackup();
                Console.WriteLine($"Loaded {song.Name}");
            }
            catch (Exception e)
            {
                // ignored
            }
        }
    }
}
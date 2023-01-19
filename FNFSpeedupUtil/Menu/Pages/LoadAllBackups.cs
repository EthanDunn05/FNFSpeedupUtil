using FNFSpeedupUtil.Helpers;

namespace FNFSpeedupUtil.Menu.Pages;

public class LoadAllBackups : Page
{
    private string ModPath { get; }

    public LoadAllBackups(string modPath)
    {
        ModPath = modPath;
    }

    protected override void Render()
    {
        var songs = ModDirectoryHelper.FindSongs(ModPath);

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
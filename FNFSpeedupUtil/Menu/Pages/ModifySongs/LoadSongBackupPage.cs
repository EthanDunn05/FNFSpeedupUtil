using FNFSpeedupUtil.SongManagement;

namespace FNFSpeedupUtil.Menu.Pages.ModifySongs;

public class LoadSongBackupPage : Page
{
    private Song Song { get; }

    public LoadSongBackupPage(Song song)
    {
        Song = song;
    }

    protected override void Render()
    {
        Song.LoadBackup();
        
        Console.WriteLine("Done loading backup!");
    }
}
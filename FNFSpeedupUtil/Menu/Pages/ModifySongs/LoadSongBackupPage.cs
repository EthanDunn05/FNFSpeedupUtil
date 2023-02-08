using FNFSpeedupUtil.SongManagement;

namespace FNFSpeedupUtil.Menu.Pages.ModifySongs;

public class LoadSongBackupPage : Page
{
    private ISong Song { get; }

    public LoadSongBackupPage(ISong song)
    {
        Song = song;
    }

    protected override void Render()
    {
        Song.LoadBackup();
        
        Console.WriteLine("Done loading backup!");
    }
}
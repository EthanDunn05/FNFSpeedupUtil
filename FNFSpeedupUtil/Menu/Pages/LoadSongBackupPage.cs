namespace FNFSpeedupUtil.Menu.Pages;

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
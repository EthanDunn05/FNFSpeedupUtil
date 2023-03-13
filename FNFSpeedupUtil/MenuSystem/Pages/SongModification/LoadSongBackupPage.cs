using FNFSpeedupUtil.Console;
using FNFSpeedupUtil.SongManagement;
using Spectre.Console;

namespace FNFSpeedupUtil.MenuSystem.Pages.SongModification;

public class LoadSongBackupPage : IPage
{
    private ISong Song { get;  }

    public LoadSongBackupPage(ISong song)
    {
        Song = song;
    }

    public void Render(Menu menu)
    {
        menu.Console.Notification("Loading Backup").Open(() =>
        {
            Song.LoadBackup();
        });
        
        menu.Console.Notification("Done Loading").Open(() => System.Console.ReadKey());
        
        menu.PreviousPage();
    }
}
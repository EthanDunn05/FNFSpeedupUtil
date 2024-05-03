using FNFSpeedupUtil.Console;
using FNFSpeedupUtil.SongManagement;
using Spectre.Console;

namespace FNFSpeedupUtil.MenuSystem.Pages.SongModification;

public class LoadSongBackupPage : IPage
{
    private ISongFiles Song { get;  }

    public LoadSongBackupPage(ISongFiles song)
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
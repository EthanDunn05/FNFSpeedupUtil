using System.IO.Abstractions;
using FNFSpeedupUtil.Console;
using FNFSpeedupUtil.SongManagement;
using Spectre.Console;

namespace FNFSpeedupUtil.MenuSystem.Pages.SongModification;

public class ModifySongPage : IPage
{
    private IDirectoryInfo ModDir { get; }
    private ISong Song { get; }

    public ModifySongPage(IDirectoryInfo modDir, ISong song)
    {
        ModDir = modDir;
        Song = song;
    }

    public void Render(Menu menu)
    {
        var navAction = menu.Console.Prompt(
            new MappedSelectionPrompt<Action>(
                "Select what to do with this song",
                new Dictionary<string, Action>
                {
                    { "Modify speed", () => menu.ChangePage(new SpeedupSongPage(Song)) },
                    {
                        "Change Scroll Speed",
                        () => menu.ChangePage(new ChangeSongScrollPage(Song))
                    },
                    { "Load Backup", () => menu.ChangePage(new LoadSongBackupPage(Song)) },
                    { "Stop modifying this song", menu.PreviousPage }
                }
            )
        );
        navAction();
    }
}

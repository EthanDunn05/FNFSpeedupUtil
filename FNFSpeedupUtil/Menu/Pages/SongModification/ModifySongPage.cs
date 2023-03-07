using System.IO.Abstractions;
using FNFSpeedupUtil.Console;
using FNFSpeedupUtil.SongManagement;
using Spectre.Console;

namespace FNFSpeedupUtil.Menu.Pages.SongModification;

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
        var navTo = menu.Console.Prompt(
            new MappedSelectionPrompt<IPage>("Select what to do with this song", new Dictionary<string, IPage>
            {
                {"Modify speed", new SpeedupSongPage(Song, ModDir)},
                {"Stop modifying this song", new ModManagePage(ModDir)}
            }));
        menu.ChangePage(navTo);
    }
}
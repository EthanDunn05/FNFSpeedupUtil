using System.IO.Abstractions;
using FNFSpeedupUtil.Console;
using FNFSpeedupUtil.Helpers;
using FNFSpeedupUtil.JsonData;
using FNFSpeedupUtil.Menu.Pages.SongModification;
using FNFSpeedupUtil.SongManagement;
using Spectre.Console;

namespace FNFSpeedupUtil.Menu.Pages;

public class ChooseSongPage : IPage
{
    private IDirectoryInfo ModDir { get; }

    public ChooseSongPage(IDirectoryInfo modDir)
    {
        ModDir = modDir;
    }

    public void Render(Menu menu)
    {
        // Load songs
        List<ISong> songs = null!;
        var speedMod = new Dictionary<ISong, ModificationData>();
        menu.Console.Notification("Loading Songs").Open(() =>
        {
            songs = ModDirectoryHelper.FindSongs(ModDir);
            songs.ForEach(s => speedMod.Add(s, s.LoadModificationData()));
        });

        // Select song
        var song = menu.Console.Prompt(new MappedSelectionPrompt<ISong>(
            "Choose a song to modify",
            songs.ToDictionary(s => $"{s.Name} [grey]({speedMod[s].SpeedModifier}x)[/]"))
        );

        menu.ChangePage(new ModifySongPage(ModDir, song));
    }
}
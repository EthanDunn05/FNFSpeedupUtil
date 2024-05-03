using System.IO.Abstractions;
using FNFSpeedupUtil.Console;
using FNFSpeedupUtil.JsonData;
using FNFSpeedupUtil.MenuSystem.Pages.SongModification;
using FNFSpeedupUtil.ModEngines;
using FNFSpeedupUtil.SongManagement;
using Spectre.Console;

namespace FNFSpeedupUtil.MenuSystem.Pages;

public class ChooseSongPage : IPage
{
    private IDirectoryInfo ModDir { get; }
    private IEngine ModEngine { get; }

    public ChooseSongPage(IDirectoryInfo modDir, IEngine modEngine)
    {
        ModDir = modDir;
        ModEngine = modEngine;
    }

    public void Render(Menu menu)
    {
        // Load songs
        List<ISongFiles> songs = null!;
        var speedMod = new Dictionary<ISongFiles, ModificationData>();
        menu.Console.Notification("Loading Songs").Open(() =>
        {
            songs = ModEngine.FindSongs(ModDir);
            songs.ForEach(s => speedMod.Add(s, s.LoadModData()));
        });

        // Select song
        var song = menu.Console.Prompt(new MappedSelectionPrompt<ISongFiles>(
            "Choose a song to modify",
            songs.ToDictionary(s => $"{s.Name} [grey]({speedMod[s].SpeedModifier}x)[/]"))
        );

        menu.ChangePage(new ModifySongPage(ModDir, song));
    }
}
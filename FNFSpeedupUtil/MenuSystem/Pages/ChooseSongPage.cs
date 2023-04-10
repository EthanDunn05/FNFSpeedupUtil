using System.IO.Abstractions;
using FNFSpeedupUtil.Console;
using FNFSpeedupUtil.Helpers;
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
    private bool OfferBack { get; }

    public ChooseSongPage(IDirectoryInfo modDir, bool offerBack, IEngine modEngine)
    {
        ModDir = modDir;
        OfferBack = offerBack;
        ModEngine = modEngine;
    }

    public void Render(Menu menu)
    {
        if (OfferBack)
        {
            var pickAnother = menu.Console.Prompt(new ConfirmationPrompt("Do you want to pick another song?"));
            if (!pickAnother)
            {
                menu.PreviousPage();
                return;
            }
        }
        
        // Load songs
        List<ISong> songs = null!;
        var speedMod = new Dictionary<ISong, ModificationData>();
        menu.Console.Notification("Loading Songs").Open(() =>
        {
            songs = ModEngine.FindSongs(ModDir);
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
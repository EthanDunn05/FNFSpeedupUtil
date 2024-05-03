using FNFSpeedupUtil.Console;
using FNFSpeedupUtil.Modifier;
using FNFSpeedupUtil.SongManagement;
using Spectre.Console;

namespace FNFSpeedupUtil.MenuSystem.Pages.SongModification;

public class SpeedupSongPage : IPage
{
    private ISongFiles Song { get; }

    public SpeedupSongPage(ISongFiles song)
    {
        Song = song;
    }

    // This method is a mess. I'll fix it later
    public void Render(Menu menu)
    {
        // Choose if pitched
        var changePitch = menu.Console.Prompt(new MappedSelectionPrompt<bool>(
            "Change Pitch?",
            new Dictionary<string, bool>
            {
                {"Change music pitch", true},
                {
                    "Do not change music pitch [red](WARNING! Speed multiplier will be rounded to one decimal place)[/]",
                    false
                },
            })
        );

        // Choose speed
        var speedModifier = menu.Console.Prompt(
            new TextPrompt<double>("Enter the speed multiplier [red](0.5 to 2.0)[/]:")
                .Validate(input => input is >= 0.5 and <= 2.0)
                .InvalidChoiceMessage("Speed multiplier must be between 0.5 and 2.0")
        );

        if (!changePitch) speedModifier = Math.Round(speedModifier, 1);

        // Modify the difficulties
        menu.Console.Status()
            .Spinner(Spinner.Known.Dots)
            .Start("Modifying Speed", ctx =>
            {
                Song.LoadBackup();
                menu.Console.MarkupLine("Restored Song");

                // Modify Difficulties
                menu.Console.MarkupLine("Modifying Speed...");
                Song.ModifySongSpeed(speedModifier, changePitch).Wait();
            });

        menu.Console.Notification("Done Modifying Song!").Open(() => System.Console.ReadKey());
        menu.PreviousPage();
    }
}
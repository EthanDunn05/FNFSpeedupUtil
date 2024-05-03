using FNFSpeedupUtil.Console;
using FNFSpeedupUtil.JsonData.OgChartData;
using FNFSpeedupUtil.SongManagement;
using Spectre.Console;

namespace FNFSpeedupUtil.MenuSystem.Pages.SongModification;

public class ChangeSongScrollPage : IPage
{
    private ISongFiles Song { get; }

    public ChangeSongScrollPage(ISongFiles song)
    {
        Song = song;
    }

    public void Render(Menu menu)
    {
        menu.Console.MarkupLine("Change Scroll Speed");

        // Prompt the new Scroll Speed
        var newSS = menu.Console.Prompt(
            new TextPrompt<double>("Enter the new scroll speed:")
                .Validate(n => n > 0)
                .InvalidChoiceMessage("You cannot use a negative scroll speed")
        );

        // Modify and Save
        menu.Console
            .Status()
            .Start(
                "Modifying difficulties",
                ctx =>
                {
                    Song.ModifyScrollSpeed(newSS);
                }
            );

        // Done
        menu.Console.Notification("Done!").Open(() => menu.Console.Input.ReadKey(false));
        menu.PreviousPage();
    }
}
using FNFSpeedupUtil.Console;
using FNFSpeedupUtil.JsonData.ChartData;
using FNFSpeedupUtil.SongManagement;
using Spectre.Console;

namespace FNFSpeedupUtil.MenuSystem.Pages.SongModification;

public class ChangeSongScrollPage : IPage
{
    private ISong Song { get; }

    public ChangeSongScrollPage(ISong song)
    {
        Song = song;
    }

    public void Render(Menu menu)
    {
        menu.Console.MarkupLine("Change Scroll Speed");
        var diffs = Song.LoadDifficulties();

        // Select a Difficulty
        var selectedDiff = menu.Console.Prompt(
            new MappedSelectionPrompt<KeyValuePair<string, JsonChart>>(
                "Which difficulty to modify?",
                (Dictionary<string, KeyValuePair<string, JsonChart>>)
                    diffs.Select(s => KeyValuePair.Create(s.Key + $"({s.Value.Song.Speed})", s))
            )
        );

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
                "Modifying difficulty",
                ctx =>
                {
                    selectedDiff.Value.Song.Speed = newSS;

                    ctx.Status("Saving Difficulty");
                    Song.SaveDifficulty(selectedDiff.Key, selectedDiff.Value);
                }
            );

        // Done
        menu.Console.Notification("Done!").Open(() => menu.Console.Input.ReadKey(false));
        menu.PreviousPage();
    }
}

using FNFSpeedupUtil.Console;
using FNFSpeedupUtil.Modifier;
using FNFSpeedupUtil.SongManagement;
using Spectre.Console;

namespace FNFSpeedupUtil.MenuSystem.Pages.SongModification;

public class SpeedupSongPage : IPage
{
    private ISong Song { get; }

    public SpeedupSongPage(ISong song)
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
            .Start("Loading Backup", ctx =>
            {
                Song.LoadBackup();
                menu.Console.MarkupLine("Restored Song");

                // Modify Difficulties
                ctx.Status("Loading Difficulties");
                var diffs = Song.LoadDifficulties();
                menu.Console.MarkupLine("Loaded Difficulties");

                ctx.Status("Modifying Difficulties");
                foreach (var (name, chart) in diffs)
                {
                    ChartModifier.ModifySpeed(chart, speedModifier);
                    Song.SaveDifficulty(name, chart);
                }
                menu.Console.MarkupLine("Modified Difficulties");

                // Modify events if it has them
                if (!Song.HasEventsChart) return;
                ctx.Status("Loading Events");
                var events = Song.LoadEvents();
                menu.Console.MarkupLine("Loaded Events");

                ctx.Status("Modifying Events");
                ChartModifier.ModifySpeed(events, speedModifier);
                Song.SaveEvents(events);
                menu.Console.MarkupLine("Modified Events");
            });

        // Modify the song files
        menu.Console.Progress().Start(ctx =>
        {
            // Make a task to modify the inst if it exists
            var modifyInstTask = Task.CompletedTask;
            if (Song.InstFile.Exists)
            {
                var instTask = ctx.AddTask("Modify Inst.ogg").MaxValue(100);

                // Create the modification task
                modifyInstTask = MusicModifier.Modify(Song.InstFile, speedModifier, changePitch,
                    (_, args) => instTask.Value(args.Percent * speedModifier));
                modifyInstTask.GetAwaiter().OnCompleted(() => instTask.Value(100));
            }

            // Make a task to modify the voices if it exists
            var modifyVoicesTask = Task.CompletedTask;
            if (Song.VoicesFile.Exists)
            {
                var voicesTask = ctx.AddTask("Modify Voices.ogg").MaxValue(100);

                // Create the modification task
                modifyVoicesTask = MusicModifier.Modify(Song.VoicesFile, speedModifier, changePitch,
                    (_, args) => voicesTask.Value(args.Percent * speedModifier));
                modifyVoicesTask.GetAwaiter().OnCompleted(() => voicesTask.Value(100));
            }

            // Wait all for processing the files in parallel
            Task.WaitAll(modifyInstTask, modifyVoicesTask);
        });

        // Really shouldn't take long, but just in case it does we show a notification
        menu.Console.Notification("Saving").Open(() =>
        {
            var modData = Song.LoadModificationData();
            modData.SpeedModifier *= speedModifier;
            Song.SaveModificationData(modData);
        });

        menu.Console.Notification("Done Modifying Song!").Open(() => System.Console.ReadKey());
        menu.PreviousPage();
    }
}
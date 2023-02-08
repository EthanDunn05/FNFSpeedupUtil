using FNFSpeedupUtil.Extensions;
using FNFSpeedupUtil.JsonData.ChartData;
using FNFSpeedupUtil.Modifier;

namespace FNFSpeedupUtil.Menu.Pages.ModifySongs;

public class ModifySpeedPage : Page
{
    private Song Song { get; }

    public ModifySpeedPage(Song song)
    {
        Song = song;
    }

    protected override void Render()
    {
        var isPitched = InputHandler.PromptBool("Should the pitch of the song be changed? (Y/N)");

        var speed = isPitched ? PromptPitchedSpeedModifier() : PromptUnpitchedSpeedModifier();

        // Modify the difficulties
        foreach (var difficulty in Song.DifficultyFiles)
        {
            var chart = difficulty.DeserializeJson<JsonChart>();
            ChartModifier.ModifySpeed(chart, speed);
            difficulty.SerializeJson(chart);
        }

        // Modify the events file if it exists
        if (Song.EventsFile != null)
        {
            var chart = Song.EventsFile.DeserializeJson<JsonChart>();
            ChartModifier.ModifySpeed(chart, speed);
            Song.EventsFile.SerializeJson(chart);
        }

        // Modify the music
        Task.Run(async () =>
        {
            await MusicModifier.Modify(Song.InstFile, speed, isPitched);
            
            if (Song.VoicesFile.Exists)
                await MusicModifier.Modify(Song.VoicesFile, speed, isPitched);
        }).Wait();
        
        // Update the modification file
        Song.ModificationData.SpeedModifier *= speed;
        Song.ModificationDataFile.SerializeJson(Song.ModificationData);

        Console.WriteLine("Done Modifying Song!");
    }

    // Kind of an ugly method, but whatever. It gets the job done
    private static double PromptPitchedSpeedModifier()
    {
        const string message = @"1.0 is normal speed | max: 2.0, min: 0.5
Enter the song speed modifier:";

        while (true)
        {
            var speed = InputHandler.PromptDouble(message);
            //var speed = (double) Math.Round(input, 1);

            if (!(speed < 0.5) && !(speed > 2)) return speed;

            Console.WriteLine("The speed you entered is invalid");
        }
    }

    private static double PromptUnpitchedSpeedModifier()
    {
        const string message = @"1.0 is normal speed | max: 2.0, min: 0.5
(Your input will be rounded to one decimal place)
Enter the song speed modifier:";

        while (true)
        {
            var input = (decimal) InputHandler.PromptDouble(message);
            var speed = (double) Math.Round(input, 1);

            if (!(speed < 0.5) && !(speed > 2)) return speed;

            Console.WriteLine("The speed you entered is invalid");
        }
    }
}
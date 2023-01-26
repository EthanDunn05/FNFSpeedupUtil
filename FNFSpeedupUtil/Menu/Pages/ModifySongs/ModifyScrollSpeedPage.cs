using FNFSpeedupUtil.ChartData;
using FNFSpeedupUtil.Modifier;

namespace FNFSpeedupUtil.Menu.Pages.ModifySongs;

public class ModifyScrollSpeedPage : Page
{
    private Song Song { get; }

    public ModifyScrollSpeedPage(Song song)
    {
        Song = song;
    }

    protected override void Render()
    {
        foreach (var difficulty in Song.DifficultyPaths)
        {
            var chart = JsonChart.Deserialize(difficulty);
            Console.WriteLine(
                $"Scroll speed: {Path.GetFileName(difficulty)} - {chart.Song.Speed}");
        }

        var scrollSpeed = InputHandler.PromptDouble("What should the new scroll speed be?");

        foreach (var difficulty in Song.DifficultyPaths)
        {
            Console.WriteLine($"Modifying {Path.GetFileName(difficulty)}");
            var chart = JsonChart.Deserialize(difficulty);
            ChartModifier.SetScrollSpeed(chart, scrollSpeed);
            chart.Serialize(difficulty);
        }


        Console.WriteLine("Done changing scroll speed!");
    }
}
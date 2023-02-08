using FNFSpeedupUtil.Extensions;
using FNFSpeedupUtil.JsonData.ChartData;
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
        foreach (var difficulty in Song.DifficultyFiles)
        {
            var chart = difficulty.DeserializeJson<JsonChart>();
            Console.WriteLine($"Scroll speed: {difficulty.Name} - {chart.Song.Speed}");
        }

        var scrollSpeed = InputHandler.PromptDouble("What should the new scroll speed be?");

        foreach (var difficulty in Song.DifficultyFiles)
        {
            Console.WriteLine($"Modifying {difficulty.Name}");
            var chart = difficulty.DeserializeJson<JsonChart>();
            ChartModifier.SetScrollSpeed(chart, scrollSpeed);
            difficulty.SerializeJson(chart);
        }

        Console.WriteLine("Done changing scroll speed!");
    }
}
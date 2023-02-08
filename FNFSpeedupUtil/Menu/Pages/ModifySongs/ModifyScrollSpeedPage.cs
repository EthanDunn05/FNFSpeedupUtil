using FNFSpeedupUtil.Extensions;
using FNFSpeedupUtil.JsonData.ChartData;
using FNFSpeedupUtil.Modifier;
using FNFSpeedupUtil.SongManagement;

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
        var difficulties = Song.LoadDifficulties();
        foreach (var (name, chart) in difficulties)
        {
            Console.WriteLine($"Scroll speed: {name} - {chart.Song.Speed}");
        }

        var scrollSpeed = InputHandler.PromptDouble("What should the new scroll speed be?");

        foreach (var (name, chart) in difficulties)
        {
            Console.WriteLine($"Modifying {name}");
            ChartModifier.SetScrollSpeed(chart, scrollSpeed);
            Song.SaveDifficulty(name, chart);
        }

        Console.WriteLine("Done changing scroll speed!");
    }
}
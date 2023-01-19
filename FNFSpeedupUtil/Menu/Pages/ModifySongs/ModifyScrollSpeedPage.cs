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
            var chart = new ChartModifier(difficulty);
            Console.WriteLine(
                $"Scroll speed: {Path.GetFileName(difficulty)} - {(double) chart.Chart["song"]!["speed"]!}");
        }

        var scrollSpeed = InputHandler.PromptDouble("What should the new scroll speed be?");

        foreach (var difficulty in Song.DifficultyPaths)
            new ChartModifier(difficulty).SetScrollSpeed(scrollSpeed);

        Console.WriteLine("Done changing scroll speed!");
    }
}
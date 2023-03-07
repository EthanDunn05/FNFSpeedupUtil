using Spectre.Console;

namespace FNFSpeedupUtil.Menu;

public class Menu
{
    public IPage Page { get; set; }
    public IAnsiConsole Console { get; }

    public Menu(IPage page, IAnsiConsole console)
    {
        Page = page;
        Console = console;
        ChangePage(page);
    }

    public void ChangePage(IPage page)
    {
        Console.Clear();
        RenderHeader();
        
        Page = page;
        Page.Render(this);
    }

    private void RenderHeader()
    {
        Console.Write(new Rule());
        Console.Write(new FigletText(FigletFont.Default, "FNF Speedup Util")
            .Justify(Justify.Center)
            .Color(Color.Purple)
        );
        Console.Write(new Rule("[blue]Made by AcidAssassin[/]")
            .Justify(Justify.Center)
        );
        Console.WriteLine();
    }
}
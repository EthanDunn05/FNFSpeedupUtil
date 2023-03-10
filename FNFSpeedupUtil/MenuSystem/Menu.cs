using System.Collections.Immutable;
using Spectre.Console;

namespace FNFSpeedupUtil.MenuSystem;

public class Menu
{
    public IPage? Page { get; set; }
    public IAnsiConsole Console { get; }
    
    /// <summary>
    /// Holds a stack with the visited pages, and the current page is not included.
    /// The bottom element of the stack will be null.
    /// </summary>
    public ImmutableStack<IPage?> PageHistory { get; private set; } = ImmutableStack<IPage?>.Empty;

    public Menu(IAnsiConsole console)
    {
        Console = console;
    }

    public void PreviousPage()
    {
        Page = PageHistory.Peek();
        PageHistory = PageHistory.Pop();
        RenderPage();
    }
    
    public void ChangePage(IPage? page)
    {
        PageHistory =PageHistory.Push(Page);
        Page = page;
        RenderPage();
    }

    private void RenderPage()
    {
        Console.Clear();
        RenderHeader();
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
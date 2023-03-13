using System.Collections.Immutable;
using Spectre.Console;

namespace FNFSpeedupUtil.MenuSystem;

public class Menu
{
    /// <summary>
    /// The Console that this menu will print to.
    /// </summary>
    public IAnsiConsole Console { get; }

    /// <summary>
    /// Holds a stack with the visited pages, and the current page is not included.
    /// The bottom element of the stack will be null.
    /// </summary>
    public ImmutableStack<IPage?> PageHistory { get; private set; } = ImmutableStack<IPage?>.Empty;
    
    private IPage? Page { get; set; }

    public Menu(IAnsiConsole console)
    {
        Console = console;
    }

    /// <summary>
    /// Goes to and renders the previous page that was visited.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when trying to go to a previous page when there isn't one. 
    /// </exception>
    public void PreviousPage()
    {
        Page = PageHistory.Peek() ?? throw new InvalidOperationException();
        PageHistory = PageHistory.Pop();
        RenderPage(Page);
    }

    /// <summary>
    /// Changes the current page, adding the current page to the history, and rendering the new one.
    /// </summary>
    /// <param name="page">The new page to navigate to</param>
    public void ChangePage(IPage page)
    {
        PageHistory = PageHistory.Push(Page);
        Page = page;
        RenderPage(Page);
    }

    private void RenderPage(IPage page)
    {
        Console.Clear();
        RenderHeader();
        page.Render(this);
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
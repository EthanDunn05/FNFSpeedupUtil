namespace FNFSpeedupUtil.Menu;

public abstract class Page
{
    protected abstract void Render();
    
    protected static void Navigate(string navigationMessage, Dictionary<string, Func<Page>> pages)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', Console.WindowWidth));
        Console.WriteLine();
        
        var names = pages.Keys.ToList();
        var selection = InputHandler.PromptList(navigationMessage, names);
        
        Console.WriteLine();
        Console.WriteLine(new string('=', Console.WindowWidth));
        Console.WriteLine();
        
        pages[names[selection]]().Render();
    }
}
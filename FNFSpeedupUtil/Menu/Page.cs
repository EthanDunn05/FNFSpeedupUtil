namespace FNFSpeedupUtil.Menu;

public abstract class Page
{
    protected abstract void Render();

    protected void NavigateOptions(string navigationMessage, Dictionary<string, Func<Page>?> options)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', Console.WindowWidth));
        Console.WriteLine();

        var names = options.Keys.ToList();
        var selection = InputHandler.PromptList(navigationMessage, names);

        var page = options[names[selection]];

        if (page != null) Navigate(page());
    }

    protected void Navigate(Page pageTo)
    {
        pageTo.Render();
        Render();
    }
}
namespace FNFSpeedupUtil.Menu.Pages;

public class MainMenuPage : Page
{
    // Tells the menu if it should be exited or not.
    // Currently is always false, but could be changed
    public bool ToExit { get; private set; }
    private string ModPath { get; }

    public MainMenuPage(string modPath)
    {
        ModPath = modPath;
    }

    // Expose Render on this class only
    public void Open() => Render();

    protected override void Render()
    {
        Navigate("What would you like to do?",new Dictionary<string, Func<Page>>
        {
            {"Modify Song", () => new ModifySongPage(ModPath)}
        });
    }
}
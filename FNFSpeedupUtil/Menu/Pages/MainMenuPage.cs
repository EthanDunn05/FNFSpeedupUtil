using System.IO.Abstractions;

namespace FNFSpeedupUtil.Menu.Pages;

public class MainMenuPage : Page
{
    // Tells the menu if it should be exited or not.
    // Currently is always false, but could be changed
    public bool ToExit { get; private set; }
    private IDirectoryInfo ModDir { get; }

    public MainMenuPage(IDirectoryInfo modDir)
    {
        ModDir = modDir;
    }

    // Expose Render on this class only
    public void Open() => Render();

    protected override void Render()
    {
        NavigateOptions("What would you like to do?",new Dictionary<string, Func<Page>?>
        {
            {"Modify Song", () => new ChooseSongPage(ModDir)},
            {"Load All Backups", () => new LoadAllBackups(ModDir) },
            {"Choose New Mod", null}
        });
    }
}
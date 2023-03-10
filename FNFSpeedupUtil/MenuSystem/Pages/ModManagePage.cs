using System.IO.Abstractions;
using FNFSpeedupUtil.Console;
using Spectre.Console;

namespace FNFSpeedupUtil.MenuSystem.Pages;

public class ModManagePage : IPage
{
    private IDirectoryInfo ModDir { get; }

    public ModManagePage(IDirectoryInfo modDir)
    {
        ModDir = modDir;
    }

    public void Render(Menu menu)
    {
        var navAction = menu.Console.Prompt(new MappedSelectionPrompt<Action>(
            "What do do with this mod?",
            new Dictionary<string, Action>
            {
                {"Manage a song", () => menu.ChangePage(new ChooseSongPage(ModDir, false))},
                {"Restore all songs", () => menu.ChangePage(new LoadAllBackupsPage(ModDir))},
                {"Use a different mod", menu.PreviousPage},
            })
        );
        navAction();
    }
}
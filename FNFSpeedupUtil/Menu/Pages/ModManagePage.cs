using System.IO.Abstractions;
using FNFSpeedupUtil.Console;
using Spectre.Console;

namespace FNFSpeedupUtil.Menu.Pages;

public class ModManagePage : IPage
{
    private IDirectoryInfo ModDir { get; }

    public ModManagePage(IDirectoryInfo modDir)
    {
        ModDir = modDir;
    }

    public void Render(Menu menu)
    {
        var nextPage = menu.Console.Prompt(new MappedSelectionPrompt<IPage>(
            "What do do with this mod?",
            new Dictionary<string, IPage>
            {
                {"Manage a song", new ChooseSongPage(ModDir)},
                {"Restore all songs", new LoadAllBackupsPage(ModDir)},
                {"Use a different mod", new MainPage(ModDir.FileSystem)},
            })
        );

        menu.ChangePage(nextPage);
    }
}
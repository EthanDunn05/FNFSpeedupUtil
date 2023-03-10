using System.IO.Abstractions;
using Spectre.Console;

namespace FNFSpeedupUtil.MenuSystem.Pages;

public class MainPage : IPage
{
    private IFileSystem FileSystem { get; }

    public MainPage(IFileSystem fileSystem)
    {
        FileSystem = fileSystem;
    }

    public void Render(Menu menu)
    {
        var modPath = menu.Console.Prompt(new TextPrompt<string>("Enter the [purple]mod folder[/]:")
            .Validate(s => FileSystem.Directory.Exists(s))
        );

        var modDir = FileSystem.DirectoryInfo.New(modPath);
        menu.ChangePage(new ModManagePage(modDir));
    }
}
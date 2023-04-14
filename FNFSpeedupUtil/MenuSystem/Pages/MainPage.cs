using System.IO.Abstractions;
using FNFSpeedupUtil.Console;
using FNFSpeedupUtil.ModEngines;
using FNFSpeedupUtil.SongManagement;
using Spectre.Console;

namespace FNFSpeedupUtil.MenuSystem.Pages;

public class MainPage : IPage
{
    private static readonly Dictionary<string, IEngine> ModEngines =
        new()
        {
            { "Vanilla/KadeEngine", new VanillaEngine() },
            { "PsychEngine", new PsychEngine() },
            { "Hypno [grey](ForeverEngine)[/]", new HypnoEngine() },
        };

    private IFileSystem FileSystem { get; }

    public MainPage(IFileSystem fileSystem)
    {
        FileSystem = fileSystem;
    }

    public void Render(Menu menu)
    {
        var modPath = menu.Console.Prompt(
            new TextPrompt<string>("Enter the [purple]mod folder[/]:").Validate(
                s => FileSystem.Directory.Exists(s)
            )
        );

        var modDir = FileSystem.DirectoryInfo.New(modPath);

        // Get the mod engine
        var possibleEngines = ModEngines
            .Where(engine => engine.Value.ValidForMod(modDir))
            .ToDictionary(d => d.Key, d => d.Value);

        // Warn if this mod is unsupported
        if (!possibleEngines.Any())
        {
            menu.Console.MarkupLine("[Red]Could not find any supported mod engines.[/]");
            menu.Console.MarkupLine(
                "[Red]Please report this [Bold](include the name of the mod)[/] to the Github page you downloaded this from.[/]"
            );
            Render(menu);
            return;
        }

        var modType = menu.Console.Prompt(
            new MappedSelectionPrompt<IEngine>("Which engine does this mod use?", possibleEngines)
        );

        menu.ChangePage(new ModManagePage(modDir, modType));
    }
}

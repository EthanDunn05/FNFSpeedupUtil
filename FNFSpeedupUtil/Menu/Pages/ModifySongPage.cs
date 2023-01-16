using FNFSpeedupUtil.Helpers;

namespace FNFSpeedupUtil.Menu.Pages;

public class ModifySongPage : Page
{
    private string ModPath { get; }

    public ModifySongPage(string modPath)
    {
        ModPath = modPath;
    }

    protected override void Render()
    {
        var songs = ModDirectoryHelper.FindSongs(ModPath);

        // Select the song to modify
        var songNames = songs.Select(s => s.Name);
        var songSelection = InputHandler.PromptList("What song to modify?", songNames);
        var song = songs[songSelection];

        // Automatically make a backup if one doesn't already exist
        if (!song.HasBackup) song.MakeBackup();

        Navigate("What would you like to do to this song?", new Dictionary<string, Func<Page>>
        {
            {"Change Speed", () => new ModifySongSpeedPage(song)},
            {"Load Backup", () => new LoadSongBackupPage(song)},
            {"Back", () => new MainMenuPage(ModPath)}
        });
    }
}
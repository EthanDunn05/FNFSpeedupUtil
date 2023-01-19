namespace FNFSpeedupUtil.Menu.Pages.ModifySongs;

public class ModifySongPage : Page
{
    private Song Song { get; }

    public ModifySongPage(Song song)
    {
        Song = song;
    }

    protected override void Render()
    {
        

        // Automatically make a backup if one doesn't already exist
        if (!Song.HasBackup) Song.MakeBackup();

        NavigateOptions("What would you like to do to this song?", new Dictionary<string, Func<Page>?>
        {
            {"Change Speed", () => new ModifySpeedPage(Song)},
            {"Change Scroll Speed", () => new ModifyScrollSpeedPage(Song)},
            {"Load Backup", () => new LoadSongBackupPage(Song)},
            {"Back", null}
        });
    }
}
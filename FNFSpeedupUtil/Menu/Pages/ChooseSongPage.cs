using FNFSpeedupUtil.Helpers;
using FNFSpeedupUtil.Menu.Pages.ModifySongs;

namespace FNFSpeedupUtil.Menu.Pages;

public class ChooseSongPage : Page
{
    private string ModPath { get; }

    public ChooseSongPage(string modPath)
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
        
        Navigate(new ModifySongPage(song));
    }
}
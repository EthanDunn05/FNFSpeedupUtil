using System.IO.Abstractions;
using FNFSpeedupUtil.Helpers;
using FNFSpeedupUtil.Menu.Pages.ModifySongs;
using FNFSpeedupUtil.SongManagement;

namespace FNFSpeedupUtil.Menu.Pages;

public class ChooseSongPage : Page
{
    private IDirectoryInfo ModDir { get; }

    public ChooseSongPage(IDirectoryInfo modDir)
    {
        ModDir = modDir;
    }

    protected override void Render()
    {
        var songs = ModDirectoryHelper.FindSongs(ModDir);

        // Select the song to modify
        var songNames = songs.Select(MakeSongDisplayName).ToList();
        songNames.Add("Back");
        
        var songSelection = InputHandler.PromptList("What song to modify?", songNames);
        
        // Back was selected
        if (songSelection == songNames.Count - 1)
            return;
        
        var song = songs[songSelection];
        
        Navigate(new ModifySongPage(song));
    }

    private static string MakeSongDisplayName(Song song)
    {
        var rawName = song.Name;
        var speedMod = song.LoadModificationData().SpeedModifier;

        return $"{rawName} ({speedMod}x)";
    }
}
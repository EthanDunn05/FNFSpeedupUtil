using System.IO.Abstractions;
using FNFSpeedupUtil.Console;
using FNFSpeedupUtil.Extensions;
using FNFSpeedupUtil.Helpers;
using FNFSpeedupUtil.SongManagement;
using Spectre.Console;

namespace FNFSpeedupUtil.MenuSystem.Pages;

public class LoadAllBackupsPage : IPage
{
    private IDirectoryInfo ModDir { get; }

    public LoadAllBackupsPage(IDirectoryInfo modDir)
    {
        ModDir = modDir;
    }

    public void Render(Menu menu)
    {
        // Load songs
        List<ISong> songs = null!;
        menu.Console.Notification("Loading Songs").Open(() => songs = ModDirectoryHelper.FindSongs(ModDir));

        menu.Console.Progress().Start(ctx =>
        {
            var task = ctx.AddTask("Loading", true, songs.Count);
            foreach (var song in songs) {
                ctx.Refresh();
                ManageExceptions.DropException(() => song.LoadBackup());
                task.Increment(1);
            }
        });

        menu.Console.Notification("Done Loading").Open(() => System.Console.ReadKey());
        
        menu.PreviousPage();
    }
}
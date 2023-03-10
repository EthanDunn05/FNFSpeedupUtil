using System.IO.Abstractions;
using FNFSpeedupUtil.MenuSystem;
using FNFSpeedupUtil.MenuSystem.Pages;
using Spectre.Console;
using Xabe.FFmpeg;

LoadFfmpeg();

var fileSystem = new FileSystem();

var menu = new Menu(AnsiConsole.Console);
menu.ChangePage(new MainPage(fileSystem));

async void LoadFfmpeg()
{
    // Find ffmpeg
    var path = Environment.GetEnvironmentVariable("PATH");
    var paths = path.Split(Path.PathSeparator);
    var ffmpegPath = paths.FirstOrDefault(s => s.Contains("ffmpeg", StringComparison.OrdinalIgnoreCase));
    
    FFmpeg.SetExecutablesPath(ffmpegPath);
}
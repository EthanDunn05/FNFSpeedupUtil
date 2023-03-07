using System.IO.Abstractions;
using FNFSpeedupUtil.Menu;
using FNFSpeedupUtil.Menu.Pages;
using Spectre.Console;
using Xabe.FFmpeg;

LoadFfmpeg();

var fileSystem = new FileSystem();

var menu = new Menu(new MainPage(fileSystem), AnsiConsole.Console);

async void LoadFfmpeg()
{
    // Find ffmpeg
    var path = Environment.GetEnvironmentVariable("PATH");
    var paths = path.Split(Path.PathSeparator);
    var ffmpegPath = paths.FirstOrDefault(s => s.Contains("ffmpeg", StringComparison.OrdinalIgnoreCase));
    
    FFmpeg.SetExecutablesPath(ffmpegPath);
}
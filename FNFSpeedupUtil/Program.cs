using System.Reflection;
using FNFSpeedupUtil.Helpers;
using FNFSpeedupUtil.Menu;
using FNFSpeedupUtil.Menu.Pages;
using FNFSpeedupUtil.Modifier;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

Console.WriteLine();
Console.WriteLine(new string('=', Console.WindowWidth));
Console.WriteLine();

Console.WriteLine("AcidAssassin's FNF Speedup Util v0.0.1");

Console.WriteLine();
Console.WriteLine(new string('=', Console.WindowWidth));
Console.WriteLine();

LoadFfmpeg();

while (true)
{

    var modPath = InputHandler.PromptDirectory("Enter the mod folder");
    var menu = new MainMenuPage(modPath);
    menu.Open();
}

async void LoadFfmpeg()
{
    // Find ffmpeg
    var path = Environment.GetEnvironmentVariable("PATH");
    var paths = path.Split(Path.PathSeparator);
    var ffmpegPath = paths.FirstOrDefault(s => s.Contains("ffmpeg", StringComparison.OrdinalIgnoreCase));
    
    FFmpeg.SetExecutablesPath(ffmpegPath);
}
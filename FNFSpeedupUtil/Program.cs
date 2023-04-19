using System.IO.Abstractions;
using System.Reflection;
using FNFSpeedupUtil.Console;
using FNFSpeedupUtil.MenuSystem;
using FNFSpeedupUtil.MenuSystem.Pages;
using Spectre.Console;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

var fileSystem = new FileSystem();

var menu = new Menu(AnsiConsole.Console);
LoadFfmpeg();

menu.ChangePage(new MainPage(fileSystem));

void LoadFfmpeg()
{
    var ffmpegPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    if (!File.Exists(ffmpegPath + "/ffmpeg") || !File.Exists(ffmpegPath + "/ffprobe"))
    {
        menu.Console.Notification("Downloading ffmpeg").Open(() =>
        {
            FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, ffmpegPath).Wait();
        });
    }

    FFmpeg.SetExecutablesPath(ffmpegPath);
}

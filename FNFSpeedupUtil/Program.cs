using System.IO.Abstractions;
using System.Reflection;
using FNFSpeedupUtil.MenuSystem;
using FNFSpeedupUtil.MenuSystem.Pages;
using Spectre.Console;
using Xabe.FFmpeg;

var fileSystem = new FileSystem();

var menu = new Menu(AnsiConsole.Console);

// Load FFmpeg
var ffmpegPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
if (!File.Exists(ffmpegPath + "/ffmpeg") || !File.Exists(ffmpegPath + "/ffprobe"))
    menu.ChangePage(new FFmpegDownloadPage(ffmpegPath));

FFmpeg.SetExecutablesPath(ffmpegPath);

menu.ChangePage(new MainPage(fileSystem));
using Spectre.Console;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace FNFSpeedupUtil.MenuSystem.Pages;

public class FFmpegDownloadPage : IPage
{
    private string FFmpegPath { get; }

    public FFmpegDownloadPage(string ffmpegPath)
    {
        FFmpegPath = ffmpegPath;
    }

    public void Render(Menu menu)
    {
        menu.Console.MarkupLine("[green]FFmpeg is being downloaded. This will only happen once.[/]");

        var progress = new Progress<ProgressInfo>();
        menu.Console.Progress()
            .Columns(
                new TaskDescriptionColumn(),
                new SpinnerColumn(),
                new ProgressBarColumn(),
                new DownloadedColumn(),
                new RemainingTimeColumn())
            .StartAsync(async ctx =>
            {
                var downloaded = ctx.AddTask("Downloading FFmpeg");

                // Start the download and add an event to change the progress bar when there is progress
                var task = FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, FFmpegPath, progress);
                progress.ProgressChanged += (_, info) =>
                {
                    // Math.Max because the progress goes backwards for some reason
                    downloaded.Value = Math.Max(info.DownloadedBytes, downloaded.Value);
                    downloaded.MaxValue = info.TotalBytes;
                };

                await task;
            }).Wait();
    }
}
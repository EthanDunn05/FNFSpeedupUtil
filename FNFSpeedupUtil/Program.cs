using FNFSpeedupUtil.Helpers;
using FNFSpeedupUtil.Modifier;
using Xabe.FFmpeg;

Run().GetAwaiter().GetResult();

async Task Run()
{
    // Find ffmpeg
    var path = Environment.GetEnvironmentVariable("PATH");
    var paths = path.Split(Path.PathSeparator);
    var ffmpegPath = paths.First(s => s.Contains("ffmpeg", StringComparison.OrdinalIgnoreCase));
    FFmpeg.SetExecutablesPath(ffmpegPath);

    // Get the mod path
    var modPath = ModDirectoryHelper.InputModFolder();

    // Select a song
    var songs = ModDirectoryHelper.FindSongs(modPath);
    var song = ModDirectoryHelper.InputSpecificSong(songs);

    // Refresh the backup to reset the chart
    song.LoadBackup();
    song.MakeBackup();

    var speedModifier = ModDirectoryHelper.InputSpeedModifier();

    Console.WriteLine();
    
    // Modify all the difficulties
    foreach (var diff in song.DifficultyPaths)
    {
        new ChartModifier(diff).ModifySpeed(speedModifier);
    }

    // Modify the events file if it exists
    if (song.EventsPath != null) new ChartModifier(song.EventsPath).ModifySpeed(speedModifier);

    // Modify the music
    await new MusicModifier(song.InstPath).Modify(speedModifier);
    await new MusicModifier(song.VoicesPath).Modify(speedModifier);
}
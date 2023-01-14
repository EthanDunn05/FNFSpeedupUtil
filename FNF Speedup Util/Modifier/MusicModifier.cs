using Xabe.FFmpeg;

namespace FNF_Speedup_Util.Modifier;

public class MusicModifier
{
    private string SongPath { get; }

    public MusicModifier(string songPath)
    {
        SongPath = songPath;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="songPath"></param>
    /// <param name="speedModification"></param>
    public async Task Modify(double speedModification)
    {
        Console.WriteLine($"Modifying music: {Path.GetFileName(SongPath)}");

        // Move file to temp location
        var bufferPath = Path.Join(Path.GetTempPath(), @"/temp.ogg");
        File.Move(SongPath, bufferPath);

        // Get the audio streams and apply speed change
        var info = await FFmpeg.GetMediaInfo(bufferPath);
        var audio = info.AudioStreams.First()
            .ChangeSpeed(speedModification);

        // Convert the temp file to the new modified version
        var conversion = FFmpeg.Conversions.New()
            .AddStream(audio)
            .SetOutput(SongPath)
            .SetOverwriteOutput(true)
            .UseMultiThread(false)
            .SetPreset(ConversionPreset.Fast);

        // Keep progress :)
        conversion.OnProgress += (sender, args) =>
        {
            Console.WriteLine(
                $"\t{args.Duration}/{args.TotalLength} : {args.Percent}%");
        };

        // Do the conversion
        var result = await conversion.Start();

        // Clean up buffer
        File.Delete(bufferPath);

        Console.WriteLine("\tDone!");
    }
}
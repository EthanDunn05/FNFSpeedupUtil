using Xabe.FFmpeg;

namespace FNFSpeedupUtil.Modifier;

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
    /// <param name="speedModification"></param>
    public async Task Modify(double speedModification, bool changePitch)
    {
        Console.WriteLine($"Modifying music: {Path.GetFileName(SongPath)}");

        // Move file to temp location
        var bufferPath = Path.Join(Path.GetTempPath(), @"/temp.ogg");
        File.Move(SongPath, bufferPath, true);

        // Get the audio streams and apply speed change
        var info = await FFmpeg.GetMediaInfo(bufferPath);
        var audio = info.AudioStreams.First();

        // Use atempo if pitch should not be changed
        if (!changePitch) audio.ChangeSpeed(speedModification);

        // Convert the temp file to the new modified version
        var conversion = FFmpeg.Conversions.New()
            .AddStream(audio)
            .SetOutput(SongPath)
            .SetOverwriteOutput(true)
            .UseMultiThread(false)
            .SetPreset(ConversionPreset.Fast);

        // Use rate changing and resampling if pitch should be changed
        if (changePitch)
        {
            conversion.AddParameter(
                $"-af \"asetrate={audio.SampleRate * speedModification},aresample={audio.SampleRate}\"");
        }

        // Keep progress :)
        conversion.OnProgress += (sender, args) =>
        {
            Console.WriteLine(
                $"{args.Duration}/{args.TotalLength / speedModification}");
        };

        // Do the conversion
        var result = await conversion.Start();

        // Clean up buffer
        File.Delete(bufferPath);
    }
}
using System.IO.Abstractions;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Events;

namespace FNFSpeedupUtil.Modifier;

public static class MusicModifier
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="speedModification"></param>
    public static async Task Modify(IFileInfo songFile, double speedModification, bool changePitch)
    {
        // Move file to temp location
        var originalPath = songFile.FullName;
        var bufferPath = Path.Join(Path.GetTempPath(), $@"/temp-{songFile.Name}.ogg");
        songFile.MoveTo(bufferPath, true);

        // Get the audio streams and apply speed change
        var info = await FFmpeg.GetMediaInfo(bufferPath);
        var audio = info.AudioStreams.First();

        // Use atempo if pitch should not be changed
        if (!changePitch) audio.ChangeSpeed(speedModification);

        // Convert the temp file to the new modified version
        var conversion = FFmpeg.Conversions.New()
            .AddStream(audio)
            .SetOutput(originalPath)
            .SetOverwriteOutput(true)
            .UseMultiThread(false)
            .SetPreset(ConversionPreset.Fast);

        // Use rate changing and resampling if pitch should be changed
        if (changePitch)
        {
            conversion.AddParameter(
                $"-af \"asetrate={audio.SampleRate * speedModification},aresample={audio.SampleRate}\"");
        }

        // Do the conversion
        var result = await conversion.Start();

        // Clean up buffer
        File.Delete(bufferPath);
    }
}
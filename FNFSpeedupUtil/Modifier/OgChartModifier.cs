using FNFSpeedupUtil.JsonData.ChartData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spectre.Console;

namespace FNFSpeedupUtil.Modifier;

/// <summary>
/// Used for editing charts.
/// </summary>
public static class OgChartModifier
{
    /// <summary>
    /// Sets the scroll speed of a song.
    /// </summary>
    /// <param name="chart">The chart to modify</param>
    /// <param name="scrollSpeed">The new scroll speed</param>
    public static void SetScrollSpeed(OgJsonChart chart, double scrollSpeed)
    {
        var song = chart.Song;
        song.Speed = scrollSpeed;
    }

    /// <summary>
    /// Modifies the speed of the chart by the given multiplier
    /// </summary>
    /// <param name="multiplier">The value to multiply the speed by</param>
    public static void ModifySpeed(OgJsonChart chart, double multiplier)
    {
        var song = chart.Song;

        // Modify song bpm
        song.Bpm *= multiplier;
        var currentBpm = song.Bpm;

        // Modify the sections
        var sections = song.Sections;
        foreach (var section in sections)
        {
            section.Bpm *= multiplier;
            currentBpm = section.Bpm ?? currentBpm;

            // Modify the notes
            foreach (var note in section.SectionNotes)
            {
                note.NoteTime /= multiplier;

                try
                {
                    note.SustainLength = ModifySustainLength(note.SustainLength, currentBpm, multiplier);
                }
                catch (Exception e)
                {
                    // ignore
                }
            }
        }

        // Modify the events
        var events = song.Events;
        if (events != null)
            foreach (var eventData in events)
                eventData.EventTime /= multiplier;
    }

    /// <summary>
    /// Modifies the length of a sustain note.
    /// </summary>
    /// <param name="originalLength">The original time the sustain is held for</param>
    /// <param name="currentBpm">The current bpm (After modification) of the song</param>
    /// <param name="multiplier">The speed multiplier to apply</param>
    /// <returns>The modified length of the sustain note</returns>
    private static int ModifySustainLength(double originalLength, double currentBpm, double multiplier)
    {
        // A more complicated process is used here since just dividing the hold time
        // doesn't work and I have no idea why...
        // FNF sustains are wack
        
        var stepLength = 60 / currentBpm * 1000 / 4;
        
        var ogBeatsHeld = (int) (originalLength / stepLength);
        var newBeatsHeld = Math.Floor(ogBeatsHeld / multiplier);
        var extraTime = originalLength % stepLength / multiplier;
        var newHoldTime = (int) (newBeatsHeld * stepLength + extraTime);

        return newHoldTime;
    }
}
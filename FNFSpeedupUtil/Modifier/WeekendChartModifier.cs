using FNFSpeedupUtil.JsonData.WeekendChartData;
using FNFSpeedupUtil.JsonData.WeekendChartData.Metadata;

namespace FNFSpeedupUtil.Modifier.Weekend;

public class WeekendChartModifier
{
    /// <summary>
    /// Modifies the speed of the chart by the given multiplier
    /// </summary>
    /// <param name="multiplier">The value to multiply the speed by</param>
    public static void ModifyChartSpeed(WeekendJsonChart chart, double multiplier)
    {
        // Modify the difficulties
        foreach (var difficulty in chart.Notes.Values)
        {
            foreach (var note in difficulty)
            {
                note.Time /= multiplier;
                note.StrumLength /= multiplier;
            }   
        }

        // Modify the events
        foreach (var eventNote in chart.Events)
        {
            eventNote.Time /= multiplier;
        }
    }

    public static void ModifyMetadata(WeekendJsonMetadata metadata, double multiplier)
    {
        foreach (var timeChange in metadata.TimeChanges)
        {
            timeChange.Time /= multiplier;
            timeChange.Bpm *= multiplier;
        }
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
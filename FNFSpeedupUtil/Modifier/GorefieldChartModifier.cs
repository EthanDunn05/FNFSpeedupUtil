using FNFSpeedupUtil.JsonData.GorefieldChartData;

namespace FNFSpeedupUtil.Modifier;

public class GorefieldChartModifier
{
    /// <summary>
    /// Modifies the speed of the chart by the given multiplier
    /// </summary>
    /// <param name="multiplier">The value to multiply the speed by</param>
    public static void ModifyChartSpeed(GorefieldJsonChart chart, double multiplier)
    {
        foreach (var strumLine in chart.StrumLines)
        {
            foreach (var note in strumLine.Notes)
            {
                note.Time /= multiplier;
                note.StrumLength /= multiplier;
            }
        }

        foreach (var eventNote in chart.Events)
        {
            eventNote.Time /= multiplier;
        }
    }

    public static void ModifyMetadata(GorefieldJsonMetadata metadata, double multiplier)
    {
        metadata.Bpm *= multiplier;
    }
}
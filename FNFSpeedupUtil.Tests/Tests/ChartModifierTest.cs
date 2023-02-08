using System.ComponentModel.DataAnnotations;
using AutoFixture.Xunit2;
using FNFSpeedupUtil.ChartData;
using FNFSpeedupUtil.Modifier;
using FNFSpeedupUtil.Tests.AutoData;

namespace FNFSpeedupUtil.Tests.Tests;

public class ChartModifierTest
{
    [Theory, AutoJsonChartData]
    public void SetScrollSpeed_DifferentScrollSpeed_ModifiedCorrectly(
        int expectedScrollSpeed, JsonChart testChart)
    {
        ChartModifier.SetScrollSpeed(testChart, expectedScrollSpeed);

        Assert.Equal(expectedScrollSpeed, testChart.Song.Speed);
    }

    [Theory, AutoJsonChartData]
    public void ModifySpeed_SongBpm_ModifiedCorrectly(
        [Range(0.5, 2)] double speedModifier, JsonChart testChart)
    {
        var expectedBpm = testChart.Song.Bpm * speedModifier;

        ChartModifier.ModifySpeed(testChart, speedModifier);

        Assert.Equal(expectedBpm, testChart.Song.Bpm);
    }

    [Theory, AutoJsonChartData]
    public void ModifySpeed_NoteTime_ModifiedCorrectly(
        [Range(0.5, 2)] double speedModifier, JsonNote testNote, JsonChart testChart)
    {
        // Arrange a test chart with the test note
        var testSection = new JsonSection {SectionNotes = {testNote}};
        testChart.Song.Sections.Insert(0, testSection);

        var expectedTime = testNote.NoteTime / speedModifier;

        ChartModifier.ModifySpeed(testChart, speedModifier);

        Assert.Equal(expectedTime, testNote.NoteTime);
    }

    [Theory, AutoJsonChartData]
    public void ModifySpeed_SustainTime_ModifiedCorrectly(JsonChart testChart)
    {
        // Arrange
        const double speedModifier = 1.5;
        const double testBpm = 120.0;
        const int testLength = 120;
        const int expectedLength = 24;
        
        var testNote = new JsonNote {0, 0, testLength};
        var testSection = new JsonSection {SectionNotes = {testNote}};
        
        testChart.Song.Bpm = testBpm;
        testChart.Song.Sections.Insert(0, testSection);

        ChartModifier.ModifySpeed(testChart, speedModifier);
        
        Assert.Equal(expectedLength, testNote.SustainLength);
    }
}
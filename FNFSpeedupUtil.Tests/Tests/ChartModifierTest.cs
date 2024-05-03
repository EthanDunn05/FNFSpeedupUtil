using System.ComponentModel.DataAnnotations;
using AutoFixture.Xunit2;
using FNFSpeedupUtil.JsonData.OgChartData;
using FNFSpeedupUtil.Modifier;
using FNFSpeedupUtil.Tests.AutoData;

namespace FNFSpeedupUtil.Tests.Tests;

public class ChartModifierTest
{
    [Theory, AutoJsonChartData]
    public void SetScrollSpeed_DifferentScrollSpeed_ModifiedCorrectly(
        int expectedScrollSpeed, OgJsonChart testChart)
    {
        OgChartModifier.SetScrollSpeed(testChart, expectedScrollSpeed);

        Assert.Equal(expectedScrollSpeed, testChart.Song.Speed);
    }

    [Theory, AutoJsonChartData]
    public void ModifySpeed_SongBpm_ModifiedCorrectly(
        [Range(0.5, 2)] double speedModifier, OgJsonChart testChart)
    {
        var expectedBpm = testChart.Song.Bpm * speedModifier;

        OgChartModifier.ModifySpeed(testChart, speedModifier);

        Assert.Equal(expectedBpm, testChart.Song.Bpm);
    }

    [Theory, AutoJsonChartData]
    public void ModifySpeed_NoteTime_ModifiedCorrectly(
        [Range(0.5, 2)] double speedModifier, OgJsonNote testNote, OgJsonChart testChart)
    {
        // Arrange a test chart with the test note
        var testSection = new OgJsonSection {SectionNotes = {testNote}};
        testChart.Song.Sections.Insert(0, testSection);

        var expectedTime = testNote.NoteTime / speedModifier;

        OgChartModifier.ModifySpeed(testChart, speedModifier);

        Assert.Equal(expectedTime, testNote.NoteTime);
    }

    [Theory, AutoJsonChartData]
    public void ModifySpeed_SustainTime_ModifiedCorrectly(OgJsonChart testChart)
    {
        // Arrange
        const double speedModifier = 1.5;
        const double testBpm = 120.0;
        const int testLength = 120;
        const int expectedLength = 24;
        
        var testNote = new OgJsonNote {0, 0, testLength};
        var testSection = new OgJsonSection {SectionNotes = {testNote}};
        
        testChart.Song.Bpm = testBpm;
        testChart.Song.Sections.Insert(0, testSection);

        OgChartModifier.ModifySpeed(testChart, speedModifier);
        
        Assert.Equal(expectedLength, testNote.SustainLength);
    }
}
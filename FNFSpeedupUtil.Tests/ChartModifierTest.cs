using FNFSpeedupUtil.ChartData;
using FNFSpeedupUtil.Menu;
using FNFSpeedupUtil.Modifier;

namespace FNFSpeedupUtil.Tests;

public class ChartModifierTest
{
    [Fact]
    public void SetScrollSpeed_DifferentScrollSpeed_ModifiedCorrectly()
    {
        var testChart = new JsonChart
        {
            Song =
            {
                Speed = 1
            }
        };
        const int newScrollSpeed = 2;

        ChartModifier.SetScrollSpeed(testChart, newScrollSpeed);
        
        Assert.Equal(testChart.Song.Speed, newScrollSpeed);
    }

    [Fact]
    public void SetScrollSpeed_NegativeSpeed_GetArgumentOutOfRangeError()
    {
        var testChart = new JsonChart
        {
            Song =
            {
                Speed = 1
            }
        };
        const int invalidScrollSpeed = -1;

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            ChartModifier.SetScrollSpeed(testChart, -1);
        });
    }

    [Fact]
    public void ModifySpeed_DoubleSpeed_ModifiedCorrectly()
    {
        var testChart = new JsonChart
        {
            
        };
    }
}
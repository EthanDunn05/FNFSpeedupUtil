 using FNFSpeedupUtil.MenuSystem;
using FNFSpeedupUtil.Tests.Mocks;
using Spectre.Console;
using Spectre.Console.Testing;

namespace FNFSpeedupUtil.Tests.Tests.MenuSystem;

public class MenuTest
{
    [Fact]
    public void ChangePage_WriteText_RendersPage()
    {
        // Arrange
        var testConsole = new TestConsole();
        var testMenu = new Menu(testConsole);

        var testPage = new MockPage(menu => { menu.Console.MarkupLine("Test Text"); });

        // Act
        testMenu.ChangePage(testPage);

        // Assert
        Assert.Contains("Test Text", testConsole.Lines[^1]);
    }

    [Fact]
    public void ChangePage_MultiplePages_TracksHistory()
    {
        // Arrange
        var testConsole = new TestConsole();
        var testMenu = new Menu(testConsole);

        var testPage1 = new MockPage(_ => { });
        var testPage2 = new MockPage(_ => { });

        var expectedHistory = new List<IPage?>
        {
            testPage2, testPage1, null
        };

        // Act
        testMenu.ChangePage(testPage1);
        testMenu.ChangePage(testPage2);
        testMenu.ChangePage(testPage1);
        
        // Assert
        Assert.Equal(expectedHistory, testMenu.PageHistory);
    }

    [Fact]
    public void PreviousPage_OnePrevious_RendersCorrectly()
    {
        // Arrange
        var testConsole = new TestConsole();
        var testMenu = new Menu(testConsole);
        var testPage = new MockPage(menu => menu.Console.MarkupLine("Test"));
        var blankPage = new MockPage(_ => { });
        
        // Act
        testMenu.ChangePage(testPage);
        testMenu.ChangePage(blankPage);
        testMenu.PreviousPage();
        
        // Assert
        Assert.Equal("Test", testConsole.Lines[^1]);
    }
    
    [Fact]
    public void PreviousPage_NoHistory_ThrowsError()
    {
        // Arrange
        var testConsole = new TestConsole();
        var testMenu = new Menu(testConsole);
        
        // Act / Assert
        Assert.Throws<InvalidOperationException>(testMenu.PreviousPage);
    }
    
    [Fact]
    public void PreviousPage_FirstPage_ThrowsError()
    {
        // Arrange
        var testConsole = new TestConsole();
        var testMenu = new Menu(testConsole);
        var blankPage = new MockPage(_ => { });
        
        // Act
        testMenu.ChangePage(blankPage);
        
        // Assert
        Assert.Throws<InvalidOperationException>(testMenu.PreviousPage);
    }
}
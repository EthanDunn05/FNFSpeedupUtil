namespace FNFSpeedupUtil.MenuSystem;

public interface IPage
{
    /// <summary>
    /// Render the page to the console.
    /// </summary>
    /// <param name="menu">The menu that this page is a part of. The menu contains the console to write to.</param>
    public void Render(Menu menu);
}

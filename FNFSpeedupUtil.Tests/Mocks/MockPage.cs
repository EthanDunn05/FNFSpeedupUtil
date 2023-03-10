using FNFSpeedupUtil.MenuSystem;

namespace FNFSpeedupUtil.Tests.Mocks;

public class MockPage : IPage
{
    private Action<Menu> OnRender { get; }

    public MockPage(Action<Menu> onRender)
    {
        OnRender = onRender;
    }

    public void Render(Menu menu)
    {
        OnRender(menu);
    }
}
using Spectre.Console;
using Spectre.Console.Rendering;

namespace FNFSpeedupUtil.Console;

public class Notification
{
    private IAnsiConsole Console { get; }
    private Table Target { get; }

    public Notification(IAnsiConsole console, string text)
    {
        Console = console;
        Target = new Table()
            .RoundedBorder()
            .Centered()
            .AddColumn(text);
    }

    public void Open(Action withOpen)
    {
        Console.Live(Target).AutoClear(true).Start(ctx =>
        {
            ctx.Refresh();
            withOpen();
        });
    }
}

public static class NotificationExtension
{
    public static Notification Notification(this IAnsiConsole @this, string text)
        => new Notification(@this, text);
}
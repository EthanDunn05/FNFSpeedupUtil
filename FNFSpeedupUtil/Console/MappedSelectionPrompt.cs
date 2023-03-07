using Spectre.Console;
using Spectre.Console.Rendering;

namespace FNFSpeedupUtil.Console;

/// <summary>
/// A prompt to extend the functionality of the basic selection prompt to have
/// mapping of a key to a value.
/// </summary>
/// <typeparam name="T">The type of value the selection items represent</typeparam>
public class MappedSelectionPrompt<T> : IPrompt<T>
{
    private string Title { get; }
    private Dictionary<string, T> Options { get; }

    public MappedSelectionPrompt(string title, Dictionary<string, T> options)
    {
        Title = title;
        Options = options;
    }

    public T Show(IAnsiConsole console)
    {
        return ShowAsync(console, CancellationToken.None).GetAwaiter().GetResult();
    }

    public Task<T> ShowAsync(IAnsiConsole console, CancellationToken cancellationToken)
    {
        var selectionPrompt = new SelectionPrompt<string>()
            .Title(Title)
            .AddChoices(Options.Keys);

        var result = selectionPrompt.ShowAsync(console, cancellationToken)
            .ContinueWith(task => Options[task.Result], cancellationToken);

        return result;
    }
}
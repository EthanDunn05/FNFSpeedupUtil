namespace FNFSpeedupUtil.Menu;

/// <summary>
/// Provides methods for easy inputting and Error checking
/// </summary>
public static class InputHandler
{
    /// <summary>
    /// Simply prompts the user with a message, and returns the string they inputted.
    /// </summary>
    /// <param name="message">What message to prompt the user with</param>
    /// <returns>The string the user entered</returns>
    public static string PromptString(string message)
    {
        Console.WriteLine(message);
        Console.Write("> ");
        return Console.ReadLine() ?? "";
    }
    
    /// <summary>
    /// Prompts a user with a message and tries to get an int in response.
    /// Will retry until a valid response is given.
    /// </summary>
    /// <param name="message">The message to prompt the user with</param>
    /// <returns>The integer the user entered</returns>
    public static int PromptInt(string message)
    {
        while (true)
        {
            var input = PromptString(message);
            if (int.TryParse(input, out var number)) return number;
            Console.WriteLine("Invalid input. Try again.");
        }
    }

    /// <summary>
    /// Prompts a user with a message and tries to get a double in response.
    /// Will retry until a valid response is given.
    /// </summary>
    /// <param name="message">The message to prompt the user with</param>
    /// <returns>The double the user entered</returns>
    public static double PromptDouble(string message)
    {
        while (true)
        {
            var input = PromptString(message);
            if (double.TryParse(input, out var number)) return number;
            Console.WriteLine("Invalid input. Try again.");
        }
    }

    /// <summary>
    /// Prompts a user with a list of options and then a message. The user will
    /// enter the index + 1 of the item. Will return the index of the item chose.
    /// Will retry until a valid response is given.
    /// </summary>
    /// <param name="message">The message to prompt the user with</param>
    /// <param name="options">A list of strings the user should be prompted with</param>
    /// <returns>The double the user entered</returns>
    public static int PromptList(string message, IEnumerable<string> options)
    {
        var optionsList = options.ToList();
        Console.WriteLine("Choose one of the following:");
        for (var i = 0; i < optionsList.Count; i++)
        {
            Console.WriteLine($"  {i + 1}) {optionsList[i]}");
        }

        while (true)
        {
            var selection = PromptInt(message) - 1;
            if (selection >= 0 && selection < optionsList.Count) return selection;
            Console.WriteLine($"{selection + 1} is not a valid item.");
        }
    }

    /// <summary>
    /// Prompts a user with a message and tries to get a path in response.
    /// Will retry until a valid response is given.
    /// </summary>
    /// <param name="message">Message to show the user</param>
    /// <returns>The directory that the user entered. Will be a directory that exists.</returns>
    public static string PromptDirectory(string message)
    {
        while (true)
        {
            var path = @"" + PromptString(message);
            if (Directory.Exists(path)) return path;
            Console.WriteLine("Folder not found, please try again");
        }
    }

    public static bool PromptBool(string message)
    {
        while (true)
        {
            var answer = PromptString(message);
            if (answer.Equals("y", StringComparison.OrdinalIgnoreCase)) return true;
            if (answer.Equals("n", StringComparison.OrdinalIgnoreCase)) return false;
            
            Console.WriteLine("Invalid Input. Try again.");
        }
    }
}
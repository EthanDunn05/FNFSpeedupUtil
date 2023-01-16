namespace FNFSpeedupUtil.Helpers;

/// <summary>
/// Helper class for getting input from the user and reading
/// the mod directory
/// TODO: Completely revamp the user input for a more convenient menu
/// </summary>
public static class ModDirectoryHelper
{
    /// <summary>
    /// Finds all of the songs in the given mod.
    /// </summary>
    /// <param name="modPath">The path to the mod folder</param>
    /// <returns>An array of all of the songs in the mod</returns>
    public static List<Song> FindSongs(string modPath)
    {
        // Check assets folder
        var assetDataPath = Path.Join(modPath, @"/assets/data");
        var assetSongFolders = Directory.GetDirectories(assetDataPath);
        if (assetSongFolders.Length > 0)
        {
            return (from dataPath in assetSongFolders
                let songName = Path.GetFileName(dataPath) ?? "unknown"
                let songPath = Path.Join(modPath, @$"/assets/songs/{songName}")
                select new Song(songName, dataPath, songPath)).ToList();
        }
        
        // Check mod folder
        var modDataPath = Path.Join(modPath, @"/mods/data");
        var modSongFolders = Directory.GetDirectories(modDataPath);
        if (modSongFolders.Length > 0)
        {
            return (from dataPath in modSongFolders
                let songName = Path.GetFileName(dataPath) ?? "unknown"
                let songPath = Path.Join(modPath, @$"/mods/songs/{songName}")
                select new Song(songName, dataPath, songPath)).ToList();
        }

        return new List<Song>();
    }

    /// <summary>
    /// Prompts the user to select a song from the given songs.
    /// Retries until a valid option is chosen.
    /// </summary>
    /// <param name="songs">The list of the songs the user can choose from</param>
    /// <returns>The song the user selected</returns>
    public static Song InputSpecificSong(List<Song> songs)
    {
        // Display all the songs in a list
        for (var i = 0; i < songs.Count; i++)
        {
            var song = songs[i];
            Console.WriteLine($"{i + 1}) {song.Name}");
        }

        Console.WriteLine("Select a song to modify");
        var choice = Console.ReadLine() ?? "";

        // Try to get a song. Try again if failed
        Song chosenSong;
        try
        {
            var selectedIndex = int.Parse(choice) - 1;
            chosenSong = songs[selectedIndex];
        }
        catch (Exception e)
        {
            Console.WriteLine("That seems to be an invalid option. Please try again");
            return InputSpecificSong(songs);
        }

        return chosenSong;
    }

    /// <summary>
    /// Gets the user to input the wanted speed modifier. The modifier
    /// is between 0 and 2.
    /// </summary>
    /// <returns>The user's inputted speed modifier</returns>
    public static double InputSpeedModifier()
    {
        Console.WriteLine("(1.0 is normal speed, max: 2.0, min: 0.5 | Will be rounded to the nearest tenths)");
        Console.WriteLine("Enter the song speed modifier");

        double speed;
        try
        {
            var input = Console.ReadLine() ?? "";
            // Round to one decimal place
            speed = (double) Math.Round(decimal.Parse(input), 1);

            // Error handle for invalid numbers
            switch (speed)
            {
                case < 0.5:
                case > 2:
                    throw new Exception();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("The speed you entered is invalid");
            return InputSpeedModifier();
        }

        return speed;
    }
}
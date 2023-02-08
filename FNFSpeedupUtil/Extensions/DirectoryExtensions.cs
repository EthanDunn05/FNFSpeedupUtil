using System.IO.Abstractions;

namespace FNFSpeedupUtil.Extensions;

public static class DirectoryExtensions
{
    // Stolen from microsoft docs
    public static void CopyTo(this IDirectoryInfo sourceDir, IDirectoryInfo destinationDir, bool recursive)
    {
        // Check if the source directory exists
        if (!sourceDir.Exists)
            throw new DirectoryNotFoundException($"Source directory not found: {sourceDir.FullName}");

        // Cache directories before we start copying
        var dirs = sourceDir.GetDirectories();

        // Create the destination directory
        destinationDir.Create();

        // Get the files in the source directory and copy to the destination directory
        foreach (var file in sourceDir.GetFiles())
        {
            var targetFile = destinationDir.File(file.Name);
            file.CopyTo(targetFile.FullName, true);
        }

        // If recursive and copying subdirectories, recursively call this method
        if (!recursive) return;
        foreach (var subDir in dirs)
        {
            var newDestinationDir = destinationDir.SubDirectory(subDir.Name);
            CopyTo(subDir, newDestinationDir, true);
        }
    }
}
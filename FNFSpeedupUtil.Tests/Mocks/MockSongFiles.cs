using System.IO.Abstractions;
using FNFSpeedupUtil.JsonData;
using FNFSpeedupUtil.SongManagement;

namespace FNFSpeedupUtil.Tests.Mocks;

/// <summary>
/// Mock for song files. Any properties used need to be set before they can be used.
/// </summary>
public class MockSongFiles : ISongFiles
{
    public MockSongFiles(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public IDirectoryInfo? DataFolder { get; set; }
    public IDirectoryInfo? MusicFolder { get; set; }
    public IFileInfo? InstFile { get; set; }
    public IFileInfo? VoicesFile { get; set; }
    public List<IFileInfo>? DifficultyFiles { get; set; }
    public IFileInfo? EventsFile { get; set; }
    public IDirectoryInfo? UtilityDataFolder { get; set; }
    public IDirectoryInfo? BackupDataFolder { get; set; }
    public IDirectoryInfo? BackupSongFolder { get; set; }
    public IFileInfo? ModificationDataFile { get; set; }
    
    public Task ModifySongSpeed(double speed, bool changePitch)
    {
        throw new NotImplementedException();
    }

    public void ModifyScrollSpeed(double scrollSpeed)
    {
        throw new NotImplementedException();
    }

    public void SaveModData(ModificationData data)
    {
        throw new NotImplementedException();
    }

    public ModificationData LoadModData()
    {
        throw new NotImplementedException();
    }

    public void SaveBackup()
    {
        throw new NotImplementedException();
    }

    public void LoadBackup()
    {
        throw new NotImplementedException();
    }
}
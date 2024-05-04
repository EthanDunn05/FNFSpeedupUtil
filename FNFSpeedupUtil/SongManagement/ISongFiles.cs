using System.IO.Abstractions;
using FNFSpeedupUtil.Extensions;
using FNFSpeedupUtil.JsonData;

namespace FNFSpeedupUtil.SongManagement;

/// <summary>
/// Controls the symbolic links and initialization of files
/// </summary>
public interface ISongFiles
{
    /// <summary>
    /// The song name (unformatted).
    /// </summary>
    public string Name { get; }
    
    public Task ModifySongSpeed(double speed, bool changePitch);

    public void ModifyScrollSpeed(double scrollSpeed);

    public void SaveModData(ModificationData data);

    public ModificationData LoadModData();

    public void SaveBackup();

    public void LoadBackup();
}
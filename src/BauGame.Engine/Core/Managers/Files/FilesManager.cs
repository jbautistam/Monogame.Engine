namespace Bau.Libraries.BauGame.Engine.Core.Managers.Files;

/// <summary>
///     Manager de archivos
/// </summary>
public class FilesManager
{
    public FilesManager(EngineManager engineManager)
    {
        EngineManager = engineManager;
        LocalFilesManager = new LocalFilesManager(this);
        StorageManager = new StorageManager(this);
    }

    /// <summary>
    ///     Manager principal
    /// </summary>
    public EngineManager EngineManager { get; }

    /// <summary>
    ///     Manager para archivos locales
    /// </summary>
    public LocalFilesManager LocalFilesManager { get; }

    /// <summary>
    ///     Manager para archivos del storage de MonoGame
    /// </summary>
    public StorageManager StorageManager { get; }
}

namespace Bau.BauEngine.Configuration;

/// <summary>
///		Configuración para el motor
/// </summary>
public class EngineSettings
{
    /// <summary>
    ///     Indica si el juego se está ejecutando en una plataforma móvil
    /// </summary>
    public bool IsMobile = OperatingSystem.IsAndroid() || OperatingSystem.IsIOS();

    /// <summary>
    ///     Indica si el juego se está ejecutando en un sistema de escritorio
    /// </summary>
    public bool IsDesktop = OperatingSystem.IsMacOS() || OperatingSystem.IsLinux() || OperatingSystem.IsWindows();

    /// <summary>
    ///     Fuente predetermianda
    /// </summary>
    public string DefaultFont { get; set; } = "";

    /// <summary>
    ///     Directorio raíz del contenido
    /// </summary>
    public required string ContentRoot { get; init; }

    /// <summary>
    ///     Título de la ventana
    /// </summary>
    public string WindowTitle { get; set; } = "Game";

    /// <summary>
    ///     Configuración de depuración
    /// </summary>
    public DebugSettings DebugSettings { get; } = new();

    /// <summary>
    ///     Configuración de pantalla
    /// </summary>
    public ScreenSettings ScreenSettings { get; } = new();

    /// <summary>
    ///     Configuración de audio
    /// </summary>
    public AudioSettings AudioSettings { get; } = new();
}

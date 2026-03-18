using Microsoft.Xna.Framework;

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
    ///     Indica si está en modo de depuración
    /// </summary>
    public bool DebugMode { get; set; }

    /// <summary>
    ///     Fuente para los textos de depuración
    /// </summary>
    public string? DebugFont { get; set; }

    /// <summary>
    ///     Color de depuración
    /// </summary>
    public Color DebugColor { get; set; } = Color.Magenta;

    /// <summary>
    ///     Color de las estadísticas
    /// </summary>
    public Color DebugOverlayColor { get; set; } = Color.Red;

    /// <summary>
    ///     Color de las imágenes
    /// </summary>
    public Color DebugImageColor { get; set; } = Color.White;

    /// <summary>
    ///     Directorio raíz del contenido
    /// </summary>
    public required string ContentRoot { get; init; }

    /// <summary>
    ///     Título de la ventana
    /// </summary>
    public string WindowTitle { get; set; } = "Game";

    /// <summary>
    ///     Configuración de pantalla
    /// </summary>
    public ScreenSettings ScreenSettings { get; } = new();

    /// <summary>
    ///     Configuración de audio
    /// </summary>
    public AudioSettings AudioSettings { get; } = new();
}

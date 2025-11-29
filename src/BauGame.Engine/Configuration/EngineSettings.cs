using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Configuration;

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
    ///     Directorio raíz del contenido
    /// </summary>
    public required string ContentRoot { get; init; }

    /// <summary>
    ///     Indica si se muestra el cursor del ratón
    /// </summary>
    public bool IsMouseVisible { get; set; } = true;

    /// <summary>
    ///     Indica si se ha configurado el juego para pantalla completa
    /// </summary>
    public bool FullScreen { get; set; } = true;

    /// <summary>
    ///     Indica si se activa el modo de cambio de resolución por hardware: evita cambios drásticos de resolución
    /// </summary>
    public bool HardwareModeSwitch { get; set; } = true;

    /// <summary>
    ///     Indica si se prefiere el multisampling (normalmente se desactiva en pixel art 2D)
    /// </summary>
    public bool PreferMultiSampling { get; set; } = false;

    /// <summary>
    ///     Indica si se va a utilizar sincronización vertical (evita el tearing)
    /// </summary>
    public bool SynchronizeWithVerticalRetrace { get; set; } = true;

    /// <summary>
    ///     Ancho de la resolución de pantalla: define el tamaño lógico del canvas, no la resolución física de la pantalla.
    /// </summary>
    public required int ScreenBufferWidth { get; init; }

    /// <summary>
    ///     Alto de la resolución de pantalla: define el tamaño lógico del canvas, no la resolución física de la pantalla.
    /// </summary>
    public required int ScreenBufferHeight { get; init; }

    /// <summary>
    ///     Ancho de la resolución de pantalla
    /// </summary>
    public required int ViewPortWidth { get; init; }

    /// <summary>
    ///     Alto de la resolución de pantalla
    /// </summary>
    public required int ViewPortHeight { get; init; }

    /// <summary>
    ///     Título de la ventana
    /// </summary>
    public string WindowTitle { get; set; } = "Game";

    /// <summary>
    ///     Indica si la ventana tiene bordes
    /// </summary>
    public bool WindowBorderless { get; set; } = true;

    /// <summary>
    ///     Indica si se le permite al usuario redimensionar la pantalla
    /// </summary>
    public bool WindowAllowUserResizing { get; set; } = true;

    /// <summary>
    ///     Direcciones de pantalla permitidas
    /// </summary>
    public DisplayOrientation DisplayOrientation { get; init; }

    /// <summary>
    ///     Ensamblado principal
    /// </summary>
    public required System.Reflection.Assembly MainAssembly { get; init; }

    /// <summary>
    ///     Carpeta de recursos
    /// </summary>
    public required string ResourceFolder { get; init; }
}

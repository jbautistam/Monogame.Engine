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
    ///     Ancho de la resolución de pantalla
    /// </summary>
    public required int ScreenWidth { get; init; }

    /// <summary>
    ///     Alto de la resolución de pantalla
    /// </summary>
    public required int ScreenHeight { get; init; }

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

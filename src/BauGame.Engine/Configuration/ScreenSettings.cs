namespace Bau.BauEngine.Configuration;

/// <summary>
///     Configuración de pantalla
/// </summary>
public class ScreenSettings
{
    /// <summary>
    ///     Indica si se muestra el cursor del ratón
    /// </summary>
    public bool IsMouseVisible { get; set; } = true;

    /// <summary>
    ///     Indica si se ha configurado el juego para pantalla completa
    /// </summary>
    public bool FullScreen { get; set; } = true;

    /// <summary>
    ///     Indica si la pantalla se dibuja sin bordes. Si es true ignora FullScreen
    /// </summary>
    public bool Borderless { get; set; } = true;

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
    public int ScreenBufferWidth { get; set; } = 1920;

    /// <summary>
    ///     Alto de la resolución de pantalla: define el tamaño lógico del canvas, no la resolución física de la pantalla.
    /// </summary>
    public int ScreenBufferHeight { get; set; } = 1080;

    /// <summary>
    ///     Ancho de la resolución de pantalla
    /// </summary>
    public int ViewPortWidth { get; set; } = 1920;

    /// <summary>
    ///     Alto de la resolución de pantalla
    /// </summary>
    public int ViewPortHeight { get; set; } = 1080;

    /// <summary>
    ///     Indica si se le permite al usuario redimensionar la pantalla
    /// </summary>
    public bool WindowAllowUserResizing { get; set; } = true;

    /// <summary>
    ///     Direcciones de pantalla permitidas
    /// </summary>
    public Microsoft.Xna.Framework.DisplayOrientation DisplayOrientation { get; set; }
}

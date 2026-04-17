using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Configuration;

/// <summary>
///		Configuración para depuración
/// </summary>
public class DebugSettings
{
    /// <summary>
    ///     Indica si está en modo de depuración
    /// </summary>
    public bool IsDebugging { get; set; }

    /// <summary>
    ///     Fuente para los textos de depuración
    /// </summary>
    public string? Font { get; set; }

    /// <summary>
    ///     Color de depuración
    /// </summary>
    public Color Color { get; set; } = Color.Magenta;

    /// <summary>
    ///     Color de las estadísticas
    /// </summary>
    public Color OverlayColor { get; set; } = Color.Red;

    /// <summary>
    ///     Color de las imágenes
    /// </summary>
    public Color ImageColor { get; set; } = Color.White;
}

using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Core.Configuration;

/// <summary>
///     Configuración de sistema
/// </summary>
public class SystemSettings
{
    /// <summary>
    ///     Volumen principal de los efectos de sonido
    /// </summary>
    public float SoundEffectsVolume { get; set; } = 1.0f;

    /// <summary>
    ///     Volumen principal de la música
    /// </summary>
    public float MusicVolume { get; set; } = 1.0f;

    /// <summary>
    ///     Idioma
    /// </summary>
    public string Language { get; set; } = "es";

    /// <summary>
    ///     Indica si el juego se muestra en pantalla completa
    /// </summary>
    public bool FullScreen { get; set; } = false;

    /// <summary>
    ///     Resolución
    /// </summary>
    public Point Resolution { get; set; } = new Point(1920, 1080);
}

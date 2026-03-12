using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering;

/// <summary>
///     Texto que se debe dibujar
/// </summary>
public class SpriteFontTextRenderCommand
{
    /// <summary>
    ///     Nombre de la fuente
    /// </summary>
    public string? Font { get; set; }

    /// <summary>
    ///     Texto
    /// </summary>
    public required string Text { get; init; }

    /// <summary>
    ///     Color
    /// </summary>
    public Color Color { get; set; } = Color.White;

    /// <summary>
    ///     Texto en negrita
    /// </summary>
    public bool Bold { get; set; }

    /// <summary>
    ///     Texto en cursiva
    /// </summary>
    public bool Italic { get; set; }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering;

/// <summary>
///		Datos de presentación
/// </summary>
public class PresentationRenderModel
{
    /// <summary>
    ///     Color
    /// </summary>
    public Color Color { get; set; } = Color.White;

    /// <summary>
    ///     Dirección de dibujo
    /// </summary>
    public SpriteEffects Effect { get; set; } = SpriteEffects.None;
}
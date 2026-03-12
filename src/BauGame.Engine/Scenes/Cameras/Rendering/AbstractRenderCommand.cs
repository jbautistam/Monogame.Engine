using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering;

/// <summary>
///     Clase base para los comandos de dibujo
/// </summary>
public abstract class AbstractRenderCommand
{
    /// <summary>
    ///     Ejecuta el comando
    /// </summary>
    public abstract void Execute(CameraDirector director, SpriteBatch spriteBatch);

    /// <summary>
    ///     Orden de presentación
    /// </summary>
    public int ZIndex { get; set; }

    /// <summary>
    ///     Efectos
    /// </summary>
    public List<Effect> Effects { get; } = [];
}
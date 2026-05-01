using Microsoft.Xna.Framework.Graphics;

namespace Bau.BauEngine.Scenes.Rendering.Transition;

/// <summary>
///     Efecto de transición: combina dos texturas
/// </summary>
public interface ITransitionEffect
{
    /// <summary>
    ///     Aplica el efecto
    /// </summary>
    void Apply(Texture2D currentScene, Texture2D nextScene, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice);

    /// <summary>
    ///     Indica si está activo
    /// </summary>
    bool Enabled { get; }

    /// <summary>
    ///     Progreso
    /// </summary>
    float Progress { get; set; }
}

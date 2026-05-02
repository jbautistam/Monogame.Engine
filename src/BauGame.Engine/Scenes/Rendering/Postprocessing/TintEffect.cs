using Bau.BauEngine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.BauEngine.Scenes.Rendering.Postprocessing;

/// <summary>
///     Efecto de cambio del color
/// </summary>
public class TintEffect(AbstractScene scene, Color overlay, float duration) : AbstractPostProcessingEffect(scene, duration)
{
    /// <summary>
    ///     Actualiza el efecto
    /// </summary>
    protected override void UpdateSelf(GameContext gameContext)
    {
    }

    /// <summary>
    ///     Aplica el efecto
    /// </summary>
    public override void Apply(RenderTarget2D source, SpriteBatch spriteBatch)
    {
        // Dibujar la textura original
        spriteBatch.Draw(source, new Rectangle(0, 0, source.Width, source.Height), Color.White);
        // Dibujar el overlay encima
        spriteBatch.Draw(Scene.RenderingManager.FiguresRenderer.WhitePixel, new Rectangle(0, 0, source.Width, source.Height), overlay);
    }
}
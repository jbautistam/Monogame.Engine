using Microsoft.Xna.Framework.Graphics;

namespace Bau.BauEngine.Scenes.Rendering.Postprocessing;

/// <summary>
///     Clase base para un efecto postproceso aplicado sobre una textura
/// </summary>
public abstract class AbstractPostProcessingEffect(float duration) : Entities.GameObjects.AbstractEntityWithDuration(duration), Entities.Common.Collections.ISecureListItem
{
    /// <summary>
    ///     Aplica el efecto de postproceso
    /// </summary>
    public abstract void Apply(RenderTarget2D source, SpriteBatch spriteBatch);
}
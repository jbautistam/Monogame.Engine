using Bau.BauEngine.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.BauEngine.Scenes.Rendering.Postprocessing;

/// <summary>
///     Clase base para un efecto postproceso aplicado sobre una textura
/// </summary>
public abstract class AbstractPostProcessingEffect : Entities.Common.Collections.ISecureListItem
{
    /// <summary>
    ///     Inicializa el efecto
    /// </summary>
    public void Start()
    {
    }

    /// <summary>
    ///     Actualiza los datos del efecto
    /// </summary>
    public abstract void Update(GameContext gameContext);

    /// <summary>
    ///     Aplica el efecto de postproceso
    /// </summary>
    public abstract void Apply(RenderTarget2D source, SpriteBatch spriteBatch);

    /// <summary>
    ///     Finaliza el efecto
    /// </summary>
    public void End(GameContext gameContext)
    {
    }

    /// <summary>
    ///     Identificador del efecto
    /// </summary>
    public string Id { get; } = Guid.NewGuid().ToString();

    /// <summary>
    ///     Indica si está activo
    /// </summary>
    public bool Enabled { get; protected set; }
}

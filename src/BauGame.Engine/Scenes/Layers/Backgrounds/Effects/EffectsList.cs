using Bau.Libraries.BauGame.Engine.Managers;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds.Effects;

/// <summary>
///		Lista de efectos
/// </summary>
public class EffectsList : Entities.Common.Collections.SecureList<AbstractBackgroundEffect>
{
    /// <summary>
    ///     Añade un elemento a la lista (en este caso no hace nada)
    /// </summary>
	protected override void Added(AbstractBackgroundEffect item)
	{
	}

    /// <summary>
    ///     Actualiza los efectos de la lista
    /// </summary>
	protected override void UpdateSelf(GameContext gameContext)
	{
        foreach (AbstractBackgroundEffect effect in Enumerate())
            if (!effect.IsCompleted)
                effect.Update(gameContext);
            else
                MarkToDestroy(effect, TimeSpan.FromMilliseconds(0));
	}

    /// <summary>
    ///     Elimina un elemento de la lista (en este caso no hace nada)
    /// </summary>
	protected override void Removed(AbstractBackgroundEffect item)
	{
	}
}

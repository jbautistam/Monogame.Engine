using Bau.Libraries.BauGame.Engine.Managers;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds.Effects;

/// <summary>
///		Lista de efectos
/// </summary>
public class EffectsList : Entities.Common.Collections.SecureList<AbstractBackgroundEffect>
{
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
}

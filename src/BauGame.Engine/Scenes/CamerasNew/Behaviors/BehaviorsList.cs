using Bau.Libraries.BauGame.Engine.Managers;

namespace Bau.Libraries.BauGame.Engine.Scenes.CamerasNew.Behaviors;

/// <summary>
///		Lista de comportamientos de cámara
/// </summary>
public class BehaviorsList : Entities.Common.Collections.SecureList<AbstractCameraBehavior>
{
    /// <summary>
    ///     Actualiza los comportamientos
    /// </summary>
	protected override void UpdateSelf(GameContext gameContext)
	{
        // Quita los comportamientos inactivos
        foreach (AbstractCameraBehavior behavior in Enumerate())
        {
            // Actualiza el comportamiento
            behavior.Update(gameContext);
            // Si ha terminado lo marca para destruir
            if (behavior.IsComplete)
                MarkToDestroy(behavior, TimeSpan.FromMilliseconds(100));
        }
	}
}

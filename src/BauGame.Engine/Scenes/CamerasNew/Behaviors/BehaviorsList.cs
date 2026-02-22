using Bau.Libraries.BauGame.Engine.Managers;

namespace Bau.Libraries.BauGame.Engine.Scenes.CamerasNew.Behaviors;

/// <summary>
///		Lista de comportamientos de cámara
/// </summary>
public class BehaviorsList : Entities.Common.Collections.SecureList<AbstractCameraBehavior>
{
    /// <summary>
    ///     Añade un elemento a la lista (en este caso no hace nada)
    /// </summary>
	protected override void Added(AbstractCameraBehavior item)
	{
	}

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

    /// <summary>
    ///     Elimina un elemento de la lista (en este caso no hace nada)
    /// </summary>
	protected override void Removed(AbstractCameraBehavior item)
	{
	}
}

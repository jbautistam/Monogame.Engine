using Bau.Libraries.BauGame.Engine.Managers;

namespace Bau.Libraries.BauGame.Engine.Actors;

/// <summary>
///		Lista de actores
/// </summary>
public class ActorsList : List<AbstractActor>
{
	// Variables privadas
	private List<(AbstractActor Actor, TimeSpan TimeToDestroy)> _actorsToRemove = [];

    /// <summary>
    ///     Elimina un actor de la capa
    /// </summary>
	public void Destroy(AbstractActor actor, TimeSpan timeToDestroy)
	{
        _actorsToRemove.Add((actor, timeToDestroy));
	}

    /// <summary>
    ///     Actualiza las físicas de los <see cref="AbstractActor"/> de la lista
    /// </summary>
	public void UpdatePhysics(GameContext gameContext)
	{
        // Elimina los actores antigous
        RemoveOld(gameContext);
        // Actualiza las físicas de los actores
        foreach (AbstractActor actor in this)
            if (actor.Enabled)
                actor.UpdatePhysics(gameContext);
	}

    /// <summary>
    ///     Elimina los actores pendientes
    /// </summary>
	private void RemoveOld(GameContext gameContext)
	{
        if (_actorsToRemove.Count > 0)
        {
            // Elimina los actores pendientes
            for (int index = _actorsToRemove.Count - 1; index >= 0; index--)
                if (gameContext.GameTime.TotalGameTime > _actorsToRemove[index].TimeToDestroy)
                {
                    // Detiene la actualización del actor
                    _actorsToRemove[index].Actor.End();
                    // Elimina el actor de la lista
                    Remove(_actorsToRemove[index].Actor);
                    // Elimina este actor de la lista
                    _actorsToRemove.RemoveAt(index);
                }
        }
	}

    /// <summary>
    ///     Actualiza los datos de los <see cref="AbstractActor"/> de la lista
    /// </summary>
    public void Update(GameContext gameContext)
	{
        foreach (AbstractActor actor in this) 
            if (actor.Enabled) 
                actor.Update(gameContext);
	}
}
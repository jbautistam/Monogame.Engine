using Bau.Libraries.BauGame.Engine.Managers;

namespace Bau.Libraries.BauGame.Engine.Actors;

/// <summary>
///		Lista de actores
/// </summary>
public class ActorsList : List<AbstractActor>
{
	// Variables privadas
    private List<AbstractActor> _actorsToAdd = [];
	private List<(AbstractActor Actor, TimeSpan TimeToDestroy)> _actorsToRemove = [];

    /// <summary>
    ///     Añade un actor a la capa pero fuera del bucle de Update para evitar los errores de "colección modificada"
    /// </summary>
	public void AddNext(AbstractActor actor)
	{
        _actorsToAdd.Add(actor);
	}

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
        // Añade los actores nuevos
        AddNewActors();
        // Elimina los actores antiguos
        RemoveOld(gameContext);
        // Actualiza las físicas de los actores
        foreach (AbstractActor actor in this)
            if (actor.Enabled)
                actor.UpdatePhysics(gameContext);
	}

    /// <summary>
    ///     Añade los nuevos actores
    /// </summary>
    private void AddNewActors()
    {
        for (int index = _actorsToAdd.Count - 1; index >= 0; index--)
        {
            // Añade el actor
            Add(_actorsToAdd[index]);
            // Arranca el actor
            _actorsToAdd[index].StartActor();
            // Elimina el actor de la lista
            _actorsToAdd.RemoveAt(index);
        }
    }

    /// <summary>
    ///     Elimina los actores pendientes
    /// </summary>
	private void RemoveOld(GameContext gameContext)
	{
        for (int index = _actorsToRemove.Count - 1; index >= 0; index--)
            if (gameContext.GameTime.TotalGameTime > _actorsToRemove[index].TimeToDestroy)
            {
                // Detiene la actualización del actor
                _actorsToRemove[index].Actor.End();
                // Elimina el actor de la lista
                Remove(_actorsToRemove[index].Actor.Id);
                // Elimina este actor de la lista
                _actorsToRemove.RemoveAt(index);
            }
	}

    /// <summary>
    ///     Elimina un actor por su Id
    /// </summary>
	private void Remove(string id)
	{
		AbstractActor? actor = this.FirstOrDefault(item => item.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase));

            if (actor is not null)
                Remove(actor);
            else
                System.Diagnostics.Debug.WriteLine("Not found");
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

    /// <summary>
    ///     Ordena los actores por su orden de presentación
    /// </summary>
    public void SortByZOrder()
    {
        // Asigna los ZOrder explícitos
        for (int index = 0; index < Count; index++)
            if (this[index].RequestedZOrder is null)
                this[index].ZOrder = index;
        // Ordena los actores por su orden
        Sort((first, second) => first.ZOrder.CompareTo(second.ZOrder));
    }
}
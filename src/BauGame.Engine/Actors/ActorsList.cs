using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace Bau.Libraries.BauGame.Engine.Actors;

/// <summary>
///		Lista de actores
/// </summary>
public class ActorsList(Scenes.Layers.AbstractLayer layer) : Entities.Common.Collections.SecureList<AbstractActor>
{
    /// <summary>
    ///     Añade un elemento a la lista
    /// </summary>
	protected override void Added(AbstractActor item)
	{
        item.ZOrder = item.RequestedZOrder ?? CountAllLife;
	}

    /// <summary>
    ///     Arranca los elementos de la lista
    /// </summary>
	public void Start()
	{
        foreach (AbstractActor actor in Enumerate())
            actor.Start();
	}

    /// <summary>
    ///     Actualiza las físicas de los <see cref="AbstractActor"/> de la lista
    /// </summary>
	public void UpdatePhysics(GameContext gameContext)
	{
        foreach (AbstractActor actor in Enumerate())
            if (actor.Enabled && actor is Interfaces.IActorPhisics actorPhisics)
                actorPhisics.UpdatePhysics(gameContext);
	}

    /// <summary>
    ///     Actualiza cuando se ha eliminado un elemento (en este caso no hace nada)
    /// </summary>
	protected override void Removed(AbstractActor item)
	{
	}

	/// <summary>
	///     Actualiza los datos de los <see cref="AbstractActor"/> de la lista
	/// </summary>
	protected override void UpdateSelf(GameContext gameContext)
	{
        foreach (AbstractActor actor in Enumerate()) 
            if (actor.Enabled) 
                actor.Update(gameContext);
	}

    /// <summary>
    ///     Dibuja los actores de la lista
    /// </summary>
	public void Draw(Camera2D camera, GameContext gameContext)
	{
        foreach (AbstractActor actor in Enumerate())
            if (actor.Enabled && actor is Interfaces.IActorDrawable actorDrawable)
                actorDrawable.Draw(camera, gameContext);
	}

    /// <summary>
    ///     Ordena los actores por su orden de presentación
    /// </summary>
    public void SortByZOrder()
    {
        Sort((first, second) => first.ZOrder.CompareTo(second.ZOrder));
    }

    /// <summary>
    ///     Capa a la que se asocia la lista
    /// </summary>
    public Scenes.Layers.AbstractLayer Layer { get; } = layer;
}
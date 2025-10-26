using Bau.Libraries.BauGame.Engine.Managers;

namespace Bau.Libraries.BauGame.Engine.Actors;

/// <summary>
///		Actor base para la implementación de un NPC
/// </summary>
public abstract class AbstractNpcActor : AbstractActor
{
	public AbstractNpcActor(Scenes.Layers.AbstractLayer layer, int zOrder) : base(layer, zOrder)
	{
		Brain = new Components.IA.BrainComponent(this);
	}

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected override void UpdateActor(GameContext gameContext)
	{
		// Actualiza el componente de IA
		Brain.Update(gameContext);
		// Actualiza el NPC
		UpdateNpcActor(gameContext);
	}

	/// <summary>
	///		Actualización adicional de los datos del actor
	/// </summary>
	protected abstract void UpdateNpcActor(GameContext gameContext);

	/// <summary>
	///		Componente para la IA del NPC
	/// </summary>
	public Components.IA.BrainComponent Brain { get; }
}

using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA;

/// <summary>
///		Componente que maneja los parámetros de una IA
/// </summary>
public class BrainComponent(AbstractActor owner) : AbstractComponent(owner, false)
{
	/// <summary>
	///		Inicia el componente
	/// </summary>
	public override void Start()
	{
		// ... no hace nada
	}

	/// <summary>
	///		Actualiza las físicas
	/// </summary>
	public override void UpdatePhysics(GameContext gameContext)
	{
		// ... no hace nada
	}

	/// <summary>
	///		Actualiza el estado
	/// </summary>
	public override void Update(GameContext gameContext)
	{
		//TODO
	}

	/// <summary>
	///		Dibuja el componente
	/// </summary>
	public override void Draw(Camera2D camera, GameContext gameContext)
	{
		// ... en este caso no hace nada
	}

	/// <summary>
	///		Finaliza el componente
	/// </summary>
	public override void End()
	{
		//TODO
	}
}

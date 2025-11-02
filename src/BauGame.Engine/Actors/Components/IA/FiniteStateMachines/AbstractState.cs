using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

/// <summary>
///		Clase base para los estados de una máquina de estados
/// </summary>
public abstract class AbstractState(string name, PropertiesState properties)
{
	/// <summary>
	///		Inicializa el estado
	/// </summary>
	public void Start(StatesMachineManager stateMachine)
	{
		// Guarda la máquina de estado
		StateMachine = stateMachine;
		// Inicializa el estado
		StartState();
	}

	/// <summary>
	///		Inicializa el estado
	/// </summary>
	protected abstract void StartState();

	/// <summary>
	///		Actualiza el estado y devuelve el siguiente
	/// </summary>
	public string? Update(Managers.GameContext gameContext)
	{
		string? nextState = null;

			// Inicializa la velocidad
			Speed = Vector2.Zero;
			// Actualiza el estado del nodo
			nextState = UpdateState(gameContext);
			// Devuelve el siguiente estado
			return nextState;
	}

	/// <summary>
	///		Actualiza el estado del nodo actual
	/// </summary>
	protected abstract string? UpdateState(Managers.GameContext gameContext);

	/// <summary>
	///		Finaliza el estado
	/// </summary>
	public abstract void End();

	/// <summary>
	///		Nombre del estado
	/// </summary>
	public string Name { get; } = name;

	/// <summary>
	///		Velocidad actual
	/// </summary>
	public Vector2 Speed { get; set; }

	/// <summary>
	///		Propiedades del estado
	/// </summary>
	public PropertiesState Properties { get; } = properties;

	/// <summary>
	///		Máquina de estados que controla este estado
	/// </summary>
	public StatesMachineManager? StateMachine { get; private set; }
}

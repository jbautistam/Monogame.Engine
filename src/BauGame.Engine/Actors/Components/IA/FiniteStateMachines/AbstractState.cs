namespace Bau.BauEngine.Actors.Components.IA.FiniteStateMachines;

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
	///		Propiedades del estado
	/// </summary>
	public PropertiesState Properties { get; } = properties;

	/// <summary>
	///		Máquina de estados que controla este estado
	/// </summary>
	public StatesMachineManager? StateMachine { get; private set; }
}

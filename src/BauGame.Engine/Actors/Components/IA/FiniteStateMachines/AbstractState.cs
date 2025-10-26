namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

/// <summary>
///		Clase base para los estados de una máquina de estados
/// </summary>
public abstract class AbstractState
{
	/// <summary>
	///		Inicializa el estado
	/// </summary>
	public void Start(StateMachine stateMachine)
	{
		StateMachine = stateMachine;
	}

	/// <summary>
	///		Inicializa el estado
	/// </summary>
	public abstract void StartState();

	/// <summary>
	///		Actualiza el estado y devuelve el siguiente
	/// </summary>
	public abstract AbstractState Update(Managers.GameContext gameContext);

	/// <summary>
	///		Finaliza el estado
	/// </summary>
	public abstract void End();

	/// <summary>
	///		Máquina de estados que controla este estado
	/// </summary>
	public StateMachine? StateMachine { get; private set; }
}

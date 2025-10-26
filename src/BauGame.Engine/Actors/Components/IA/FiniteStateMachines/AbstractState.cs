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
	public abstract string? Update(Managers.GameContext gameContext);

	/// <summary>
	///		Arranca una animación
	/// </summary>
	protected void StartAnimation(PropertiesState properties)
	{
		AbstractActor? owner = StateMachine?.Brain.Owner;

			if (owner is not null)
			{
				if (string.IsNullOrWhiteSpace(properties.Animation))
				{
					owner.Renderer.Texture = properties.Texture;
					owner.Renderer.Region = properties.Region;
				}
				else
				{
					owner.Renderer.Animator.Reset();
					owner.Renderer.StartAnimation(properties.Texture, properties.Animation, properties.AnimationLoop);
				}
			}
	}

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

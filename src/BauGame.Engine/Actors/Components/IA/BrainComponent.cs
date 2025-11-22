using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA;

/// <summary>
///		Componente que maneja los parámetros de una IA
/// </summary>
public class BrainComponent : AbstractComponent
{
	public BrainComponent(AbstractActor owner) : base(owner, false)
	{
		StatesMachineManager = new FiniteStateMachines.StatesMachineManager(this);
		AgentSteeringManager = new Steering.AgentSteeringManager(this, null);
	}

	/// <summary>
	///		Inicia el componente
	/// </summary>
	public override void Start()
	{
		StatesMachineManager.Start(null);
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
		Health.HealthComponent health = GetHealth();

			// Actualiza la máquina de estados
			StatesMachineManager.Update(gameContext);
			// Comprueba la salud
			if (health.JustDead)
			{
				// Elimina el actor
				KillActor(gameContext);
				// Indica que no acaba de morir
				health.JustDead = false;
			}
			else if (!health.IsDead)
				Move(gameContext);
	}

	/// <summary>
	///		Obtiene el componente de salud del actor. Si no existe crea uno
	/// </summary>
	private Health.HealthComponent GetHealth()
	{
		Health.HealthComponent? health = Owner.Components.GetComponent<Health.HealthComponent>();

			// Crea un componente de salud si no existía (para no tener que preguntar si es nulo)
			if (health is null)
				health = new Health.HealthComponent(Owner)
								{
									Health = 100,
									Lives = 1,
									InvulnerabilityTime = 0
								};
			// Devuelve el componente
			return health;
	}

	/// <summary>
	///		Mueve el actor
	/// </summary>
	private void Move(GameContext gameContext)
	{
		if (StatesMachineManager.Current is not null)
		{
			Owner.Transform.Bounds.Translate(StatesMachineManager.Current.Speed * gameContext.DeltaTime);
			Owner.Transform.Bounds.Clamp(Owner.Layer.Scene.WorldBounds);
		}
	}

	/// <summary>
	///		Elimina el actor
	/// </summary>
	private void KillActor(GameContext gameContext)
	{
		Physics.CollisionComponent? collision = Owner.Components.GetComponent<Physics.CollisionComponent>();

			// Desactiva la colisión
			collision?.ToggleEnabled(false);
			// Elimina el actor
			Owner.Layer.Actors.Destroy(Owner, gameContext.GetTotalTime(TimeSpan.FromSeconds(5)));
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

	/// <summary>
	///		Controlador de la máquina de estados
	/// </summary>
	public FiniteStateMachines.StatesMachineManager StatesMachineManager { get; }

	/// <summary>
	///		Controlador para movimientos
	/// </summary>
	public Steering.AgentSteeringManager AgentSteeringManager { get; }
}

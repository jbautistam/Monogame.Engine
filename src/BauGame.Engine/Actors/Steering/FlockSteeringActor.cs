using Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;
using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace Bau.Libraries.BauGame.Engine.Actors.Steering;

/// <summary>
///     Manager para los componentes de movimiento de una bandada
/// </summary>
public class FlockSteeringActor(Scenes.Layers.AbstractLayer layer) : AbstractActor(layer, 0)
{
    /// <summary>
    ///     Añade un comportamiento a la lista
    /// </summary>
    public void Add(AbstractSteeringBehavior steeringBehavior, float weight)
    {
        Behaviors.Add(new AgentSteeringManager.Behavior(steeringBehavior, weight));
    }

    /// <summary>
    ///     Inicializa el actor
    /// </summary>
	public override void StartActor()
	{
	}

    /// <summary>
    ///     Actualiza el componente
    /// </summary>
    protected override void UpdateActor(GameContext gameContext)
    {
        // Asigna los máximos definidos en la bandada
        //? No actualiza los agentes porque realmente son componentes dentro de otros actores
        foreach (AgentSteeringManager agent in Agents)
        {
            agent.MaxSpeed = MaxSpeed;
            agent.MaxForce = MaxForce;
        }
    }

    /// <summary>
    ///     Dibuja el actor
    /// </summary>
	protected override void DrawActor(Camera2D camera, GameContext gameContext)
	{
		// ... no hace nada, simplemente implementa la interface
	}

    /// <summary>
    ///     Finaliza el actor
    /// </summary>
	protected override void EndActor()
	{
        // ... no hace nada, simplemente implementa la interface
	}

    /// <summary>
    ///     Comportamientos definidos
    /// </summary>
    public List<AgentSteeringManager.Behavior> Behaviors { get; } = [];

    /// <summary>
    ///     Agentes que conforman la bandada
    /// </summary>
    public List<AgentSteeringManager> Agents { get; } = [];

    /// <summary>
    ///     Velocidad máxima
    /// </summary>
    public float MaxSpeed { get; set; } = 100f;

    /// <summary>
    ///     Fuerza máxima aplicada
    /// </summary>
    public float MaxForce { get; set; } = 10f;
}
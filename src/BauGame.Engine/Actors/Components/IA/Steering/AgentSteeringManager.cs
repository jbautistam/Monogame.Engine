using Bau.Libraries.BauGame.Engine.Managers;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

/// <summary>
///     Manager de los componentes de movimiento
/// </summary>
public class AgentSteeringManager(BrainComponent brain)
{
    // Datos del comportamiento
    public record Behavior(AbstractSteeringBehavior SteeringBehavior, float Weight);

    /// <summary>
    ///     Añade un comportamiento a la lista
    /// </summary>
    public void Add(AbstractSteeringBehavior steeringBehavior, float weight)
    {
        Behaviors.Add(new Behavior(steeringBehavior, weight));
    }

	/// <summary>
	///		Obtiene la posición
	/// </summary>
	public Vector2 Position => Brain.Owner.Transform.Bounds.TopLeft;

    /// <summary>
    ///     Actualiza el componente
    /// </summary>
    public void Update(GameContext gameContext)
    {
        Vector2 force = Vector2.Zero;

            // Calcula las fuerzas
            foreach (Behavior behavior in Behaviors)
                force += behavior.SteeringBehavior.Calculate(this) * behavior.Weight;
            // Limita la fuerza aplicada
            if (force.Length() > MaxForce)
                force = Vector2.Normalize(force) * MaxForce;
            // Asigna la aceleración
            Acceleration = force;
            // Calcula la velocidad
            Velocity += Acceleration * gameContext.DeltaTime;
            // Limita la velocidad
            if (Velocity.Length() > MaxSpeed)
                Velocity = Vector2.Normalize(Velocity) * MaxSpeed;
            // Cambia la posición del actor
            Brain.Owner.Transform.Bounds.Translate(Velocity * gameContext.DeltaTime);
    }

    /// <summary>
    ///     Componente principal de IA
    /// </summary>
    public BrainComponent Brain { get; } = brain;

    /// <summary>
    ///     Comportamientos definidos
    /// </summary>
    public List<Behavior> Behaviors { get; } = [];

    /// <summary>
    ///     Agentes que conforman la bandada
    /// </summary>
    public List<AgentSteeringManager> Agents { get; } = [];

    /// <summary>
    ///     Velocidad actual
    /// </summary>
    public Vector2 Velocity { get; set; }

    /// <summary>
    ///     Aceleración
    /// </summary>
    public Vector2 Acceleration { get; set; }

    /// <summary>
    ///     Velocidad máxima
    /// </summary>
    public float MaxSpeed { get; set; } = 100f;

    /// <summary>
    ///     Fuerza máxima aplicada
    /// </summary>
    public float MaxForce { get; set; } = 10f;
}
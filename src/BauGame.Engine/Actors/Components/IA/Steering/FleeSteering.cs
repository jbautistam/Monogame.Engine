using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

/// <summary>
///     Comportamiento de un movimiento de separación de un punto
/// </summary>
public class FleeSteering : AbstractSteeringBehavior
{
    /// <summary>
    ///     Calcula el vector
    /// </summary>
    public override Vector2 Calculate(AgentSteeringManager agentSteeringManager) => FleeFrom(agentSteeringManager, Target, MaximumDistance);

    /// <summary>
    ///     Punto del que se va a separar
    /// </summary>
    public Vector2 Target { get; set; }

    /// <summary>
    ///     Distancia máxima al punto
    /// </summary>
    public float MaximumDistance { get; set; } = 50f;
}
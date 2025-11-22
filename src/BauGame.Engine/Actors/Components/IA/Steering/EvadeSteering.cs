using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

/// <summary>
///     Comportamiento para separarse de un objetivo
/// </summary>
public class EvadeSteering : AbstractSteeringBehavior
{
    /// <summary>
    ///     Calcula el movimiento
    /// </summary>
    public override Vector2 Calculate(AgentSteeringManager agentSteeringManager)
    {
        Vector2 toTarget = Target - agentSteeringManager.Position;
        float lookAheadTime = toTarget.Length() / agentSteeringManager.MaxSpeed;
        Vector2 predictedPos = Target + agentSteeringManager.Velocity * lookAheadTime;

            // Calcula la fuerza de separación
            return FleeFrom(agentSteeringManager, predictedPos, MaximumDistance);
    }

    /// <summary>
    ///     Punto que se quiere evitar
    /// </summary>
    public Vector2 Target { get; set; }

    /// <summary>
    ///     Distancia máxima al punto
    /// </summary>
    public float MaximumDistance { get; set; } = 50f;
}
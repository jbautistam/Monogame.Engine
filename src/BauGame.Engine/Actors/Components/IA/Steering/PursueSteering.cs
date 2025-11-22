using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

/// <summary>
///     Comportamiento para perseguir un objetivo
/// </summary>
public class PursueSteering : AbstractSteeringBehavior
{
    /// <summary>
    ///     Calcula la fuerza necesaria
    /// </summary>
    public override Vector2 Calculate(AgentSteeringManager agentSteeringManager)
    {
        if (Actor is null)
            return Vector2.Zero;
        else
        {
            Vector2 targetPos = Actor.Transform.Bounds.TopLeft;
            Vector2 toTarget = targetPos - agentSteeringManager.Position;
            float lookAheadTime = toTarget.Length() / agentSteeringManager.MaxSpeed;
            Vector2 predictedPos = targetPos + agentSteeringManager.Velocity * lookAheadTime;

                // Calcula la fuerza para llegar al punto previsto
                return AddressTo(agentSteeringManager, predictedPos, MinimumDistance);
        }
    }

    /// <summary>
    ///     Actor que se va a perseguir
    /// </summary>
    public AbstractActor? Actor { get; set; }

    /// <summary>
    ///     Distancia mínima para considerar alcanzado el objetivo
    /// </summary>
    public float MinimumDistance { get; set; } = 100f;
}
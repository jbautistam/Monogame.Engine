using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

/// <summary>
///     Calculo de movimiento para un comportamiento de bandada
/// </summary>
public class FlockSteering : AbstractSteeringBehavior
{
    /// <summary>
    ///     Calcula el movimiento necesario para un efecto de bandada
    /// </summary>
    public override Vector2 Calculate(AgentSteeringManager agentSteeringManager)
    {
        Vector2 separation = new SeparationSteering().Calculate(agentSteeringManager) * 1.5f;
        Vector2 alignment = new AlignmentSteering().Calculate(agentSteeringManager);
        Vector2 cohesion = new CohesionSteering().Calculate(agentSteeringManager);

            // Devuelve el movimiento
            return separation + alignment + cohesion;
    }
}
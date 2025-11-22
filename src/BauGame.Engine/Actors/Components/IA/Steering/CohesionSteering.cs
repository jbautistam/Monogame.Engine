using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

/// <summary>
///     Movimiento para juntar agentes
/// </summary>
public class CohesionSteering : AbstractSteeringBehavior
{
    /// <summary>
    ///     Calcula el movimiento
    /// </summary>
    public override Vector2 Calculate(AgentSteeringManager agentSteeringManager)
    {
        Vector2 steering = Vector2.Zero;
        int count = 0;

            // Calcula la distancia con los agentes cercanos
            if (agentSteeringManager.FlockSteeringActor is not null)
                foreach (AgentSteeringManager other in agentSteeringManager.FlockSteeringActor.Agents)
                {
                    if (other != agentSteeringManager)
                    {
                        float distance = Vector2.Distance(agentSteeringManager.Position, other.Position);

                            // Si está dentro de la distancia, añade la posición
                            if (distance > 0 && distance < NeighborRadius)
                            {
                                steering += other.Position;
                                count++;
                            }
                    }
                }
            // Si realmente tenemos algún vecino
            if (count > 0)
            {
                // Calcula la posición del centro de masa
                steering /= count;
                // Calcula el movimiento necesario para acercarse al punto de masa
                steering = AddressTo(agentSteeringManager, steering, NeighborRadius);
            }
            // Devuelve el movimiento
            return steering;
    }

    /// <summary>
    ///     Radio de cercanía a los vecinos
    /// </summary>
    private float NeighborRadius { get; set; } = 50f;
}
using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Tools.Extensors;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

/// <summary>
///     Comportamiento para alineamiento con otros actores
/// </summary>
public class AlignmentSteering : AbstractSteeringBehavior
{
    /// <summary>
    ///     Calcula el movimiento
    /// </summary>
    public override Vector2 Calculate(AgentSteeringManager agentSteeringManager)
    {
        Vector2 steering = Vector2.Zero;
        int count = 0;

            // Calcula el vector de movimiento contra otros actores
            if (agentSteeringManager.FlockSteeringActor is not null)
                foreach (AgentSteeringManager other in agentSteeringManager.FlockSteeringActor.Agents)
                    if (other != agentSteeringManager)
                    {
                        float distance = Vector2.Distance(agentSteeringManager.Position, other.Position);

                            // Cambia la velocidad si está en el rango
                            if (distance > 0 && distance < NeighborRadius)
                            {
                                steering += other.Velocity;
                                count++;
                            }
                    }
            // Si realmente se ha encontrado algún actor
            if (count > 0)
            {
                // Calcula la velocidad de movimiento mínima
                steering /= count;
                if (steering.Length() > 0)
                {
                    // Normaliza la velocidad
                    steering = Vector2.Normalize(steering) * agentSteeringManager.MaxSpeed;
                    // Añade la velocidad
                    steering -= agentSteeringManager.Velocity;
                    // y la limita
                    steering = steering.ClampComponents(agentSteeringManager.MaxForce);
                }
            }
            // Devuelve el vector calculado
            return steering;
    }

    /// <summary>
    ///     Distancia con los vecinos
    /// </summary>
    public float NeighborRadius { get; set; } = 50f;
}
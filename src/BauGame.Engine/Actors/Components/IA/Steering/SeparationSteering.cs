using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Tools.Extensors;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

/// <summary>
///     Comportamiento para separase del resto de los agentes
/// </summary>
public class SeparationSteering : AbstractSteeringBehavior
{
    /// <summary>
    ///     Calcula el movimiento
    /// </summary>
    public override Vector2 Calculate(AgentSteeringManager agentSteeringManager)
    {
        Vector2 steering = Vector2.Zero;
        int count = 0;

            // Calcula la fuerza que debe aplicar con respecto al resto
            if (agentSteeringManager.FlockSteeringActor is not null)
                foreach (AgentSteeringManager other in agentSteeringManager.FlockSteeringActor.Agents)
                    if (other != agentSteeringManager)
                    {
                        float distance = Vector2.Distance(agentSteeringManager.Position, other.Position);

                            if (distance > 0 && distance < SeparationDistance)
                            {
                                Vector2 diff = agentSteeringManager.Position - other.Position;

                                    // Normaliza la distancia
                                    diff.Normalize();
                                    // Calcula una fuerza inversa a la distancia
                                    diff /= distance;
                                    // y la acumula
                                    steering += diff;
                                    // Incrementa el número de elementos
                                    count++;
                            }
                    }
            // Si realmente tenemos algo
            if (count > 0)
            {
                // Calcula la fuerza media
                steering /= count;
                // Cambia la velocidad
                if (steering.Length() > 0)
                {
                    steering = Vector2.Normalize(steering) * agentSteeringManager.MaxSpeed;
                    steering -= agentSteeringManager.Velocity;
                    steering = steering.ClampComponents(agentSteeringManager.MaxForce);
                }
            }
            // Devuelve la fuerza
            return steering;
    }

    /// <summary>
    ///     Distancia de separación
    /// </summary>
    public float SeparationDistance { get; set; } = 50f;
}
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

/// <summary>
///     Comportamiento para evitar obstáculos
/// </summary>
public class ObstacleAvoidanceSteering : AbstractSteeringBehavior
{
    /// <summary>
    ///     Calcula la fuerza
    /// </summary>
    public override Vector2 Calculate(AgentSteeringManager agentSteeringManager)
    {
        Vector2 force = Vector2.Zero;

            // Calcula la separación con los obstáculos
            foreach (Models.Circle obstacle in Obstacles)
            {
                float distance = Vector2.Distance(agentSteeringManager.Position, obstacle.Center);

                    // Calcula la fuerza para evitar el obstáculo
                    if (distance < AvoidDistance + obstacle.Radius)
                    {
                        Vector2 direction = agentSteeringManager.Position - obstacle.Center;

                            // Normaliza la dirección
                            direction.Normalize();
                            // Cambia la dirección para que cuanto más cerca esté, más fuerza de separación le aplique
                            direction *= AvoidDistance + obstacle.Radius - distance;
                            // Añade la dirección a la fuerza
                            force += direction;
                    }
            }
            // Devuelve la fuerza calculada
            return force;
    }

    /// <summary>
    ///     Puntos que definen los obstáculos
    /// </summary>
    public List<Models.Circle> Obstacles { get; } = [];

    /// <summary>
    ///     Distnacia para evitar los obstáculos
    /// </summary>
    public float AvoidDistance { get; set; } = 100f;
}
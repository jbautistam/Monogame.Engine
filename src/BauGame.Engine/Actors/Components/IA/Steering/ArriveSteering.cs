using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

/// <summary>
///     Control de movimiento para llegada a un punto
/// </summary>
public class ArriveSteering : AbstractSteeringBehavior
{
    /// <summary>
    ///     Calcula el movimiento
    /// </summary>
    public override Vector2 Calculate(AgentSteeringManager agentSteeringManager)
    {
        Vector2 steer = Vector2.Zero;
        Vector2 toTarget = Target - agentSteeringManager.Position;
        float distance = toTarget.Length();

            // Si todavía nos encontramos a la distancia de búsqueda del objetivo
            if (distance > MinimumDistance)
            {
                Vector2 desired = Vector2.Normalize(toTarget);

                    // Caclula la velocidad deseada
                    if (distance < SlowingDistance)
                        desired *= agentSteeringManager.MaxSpeed * (distance / SlowingDistance);
                    else
                        desired *= agentSteeringManager.MaxSpeed;
                    // Calcula el movimiento
                    steer = desired - agentSteeringManager.Velocity;
            }
            // Devuelve la fuerza calculada
            return steer;
    }

    /// <summary>
    ///     Punto de destino
    /// </summary>
    public Vector2 Target { get; set; }

    /// <summary>
    ///     Distancia mínima para considerar alcanzado el objetivo
    /// </summary>
    public float MinimumDistance { get; set; } = 100f;

    /// <summary>
    ///     Distancia a partir de la que empieza a frenar
    /// </summary>
    public float SlowingDistance { get; set; } = 100f;
}
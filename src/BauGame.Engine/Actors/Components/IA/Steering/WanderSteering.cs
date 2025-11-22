using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

/// <summary>
///     Comportamiento para orbitar alrededor de un punto
/// </summary>
public class WanderSteering : AbstractSteeringBehavior
{
    /// <summary>
    ///     Calcula el movimiento
    /// </summary>
    public override Vector2 Calculate(AgentSteeringManager agentSteeringManager)
    {
        // Agrega un poco de aletoriedad al punto de destino
        Target += new Vector2(WanderJitter * Tools.Randomizer.GetRandom(-0.5f, 0.5f), WanderJitter * Tools.Randomizer.GetRandom(-0.5f, 0.5f));
        Target = Vector2.Normalize(Target) * WanderRadius;
        // Devuelve el punto rotado
        return Vector2.Transform(Target + new Vector2(WanderDistance, 0), Matrix.CreateRotationZ(Angle));
    }

    /// <summary>
    ///     Punto destino
    /// </summary>
    public Vector2 Target { get; set; }

    /// <summary>
    ///     Radio de órbita
    /// </summary>
    public float WanderRadius { get; set; } = 25f;

    /// <summary>
    ///     Distancia de órbita
    /// </summary>
    public float WanderDistance { get; set; } = 50f;

    /// <summary>
    ///     Factor de aletoriedad
    /// </summary>
    public float WanderJitter { get; set; } = 25f;

    /// <summary>
    ///     Angulo de orientación
    /// </summary>
    public float Angle { get; set; } = 1f;
}
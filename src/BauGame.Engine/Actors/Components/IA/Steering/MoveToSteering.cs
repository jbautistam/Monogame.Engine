using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

/// <summary>
///     Control de movimiento en una dirección
/// </summary>
public class MoveToSteering : AbstractSteeringBehavior
{
    /// <summary>
    ///     Calcula el movimiento
    /// </summary>
    public override Vector2 Calculate(AgentSteeringManager agentSteeringManager) => Direction * agentSteeringManager.MaxSpeed;

    /// <summary>
    ///     Dirección
    /// </summary>
    public Vector2 Direction { get; set; }
}
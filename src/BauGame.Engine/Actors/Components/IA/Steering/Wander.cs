using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;
public class Wander : ISteeringBehavior
{
    private Vector2 wanderTarget = new Vector2(1, 0);
    private float wanderRadius = 25f;
    private float wanderDistance = 50f;
    private float wanderJitter = 25f;

    public Vector2 Calculate(Agent agent)
    {
        // Agregar un poco de jitter al wanderTarget
        wanderTarget += new Vector2(
            (float)(Random.Shared.NextDouble() - 0.5) * wanderJitter,
            (float)(Random.Shared.NextDouble() - 0.5) * wanderJitter
        );
        wanderTarget = Vector2.Normalize(wanderTarget) * wanderRadius;

        Vector2 target = wanderTarget + new Vector2(wanderDistance, 0);
        target = Vector2.Transform(target, Matrix.CreateRotationZ(agent.Orientation));

        return target;
    }
}
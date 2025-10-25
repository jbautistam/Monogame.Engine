using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

public class BlendedSteering
{
    private List<(ISteeringBehavior behavior, float weight)> behaviors;

    public Vector2 Calculate(Agent agent)
    {
        Vector2 force = Vector2.Zero;
        foreach (var (behavior, weight) in behaviors)
        {
            force += behavior.Calculate(agent) * weight;
        }
        return Vector2.Clamp(force, -agent.MaxForce, agent.MaxForce);
    }
}
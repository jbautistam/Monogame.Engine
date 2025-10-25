using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

public class Arrive : ISteeringBehavior
{
    private System.Func<Vector2> targetFunc;
    private float slowingDistance = 100f;

    public Arrive(System.Func<Vector2> targetFunc, float slowingDistance = 100f)
    {
        this.targetFunc = targetFunc;
        this.slowingDistance = slowingDistance;
    }

    public Vector2 Calculate(Agent agent)
    {
        Vector2 toTarget = targetFunc() - agent.Position;
        float distance = toTarget.Length();

        if (distance < 0.1f) return Vector2.Zero;

        Vector2 desired = Vector2.Normalize(toTarget);
        if (distance < slowingDistance)
        {
            desired *= agent.MaxSpeed * (distance / slowingDistance);
        }
        else
        {
            desired *= agent.MaxSpeed;
        }

        Vector2 steer = desired - agent.Velocity;
        return steer;
    }
}
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

public class Seek1 : ISteeringBehavior
{
    private System.Func<Vector2> targetFunc;

    public Seek1(System.Func<Vector2> targetFunc)
    {
        this.targetFunc = targetFunc;
    }

    public Vector2 Calculate(Agent agent)
    {
        Vector2 desired = targetFunc() - agent.Position;
        if (desired.Length() == 0) return Vector2.Zero;

        desired = Vector2.Normalize(desired) * agent.MaxSpeed;
        Vector2 steer = desired - agent.Velocity;
        return steer;
    }
}
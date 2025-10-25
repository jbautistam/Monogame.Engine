using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

public class Pursue : ISteeringBehavior
{
    private System.Func<Vector2> targetFunc;

    public Pursue(System.Func<Vector2> targetFunc)
    {
        this.targetFunc = targetFunc;
    }

    public Vector2 Calculate(Agent agent)
    {
        Vector2 targetPos = targetFunc();
        Vector2 toTarget = targetPos - agent.Position;
        float distance = toTarget.Length();
        float lookAheadTime = distance / agent.MaxSpeed;

        // Predecir la posición futura del objetivo
        Vector2 predictedPos = targetPos + agent.Velocity * lookAheadTime;

        return new Seek(() => predictedPos).Calculate(agent);
    }
}
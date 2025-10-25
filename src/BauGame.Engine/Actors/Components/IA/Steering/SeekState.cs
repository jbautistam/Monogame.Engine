using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

public class SeekState : SteeringState
{
    private Vector2 target;

    public SeekState(Agent agent, Vector2 target) : base(agent)
    {
        this.target = target;
    }

    public override void Enter()
    {
        SetBehaviorWeight(new Seek(() => target), 1.5f);
        SetBehaviorWeight(new Wander(), 0.1f);
    }

    public override void Update(float deltaTime)
    {
        if (Vector2.Distance(agent.Position, target) < 10f)
        {
            agent.ChangeState(new WanderState(agent));
        }
    }
}

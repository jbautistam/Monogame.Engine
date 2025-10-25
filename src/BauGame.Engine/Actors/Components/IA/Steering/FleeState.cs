using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

public class FleeState : SteeringState
{
    private Vector2 fleeTarget = new Vector2(400, 300); // posición del enemigo

    public FleeState(Agent agent) : base(agent) { }

    public override void Enter()
    {
        SetBehaviorWeight(new Flee(() => fleeTarget), 2.0f);
        SetBehaviorWeight(new Wander(), 0.2f);
    }

    public override void Update(float deltaTime)
    {
        if (Vector2.Distance(agent.Position, fleeTarget) > 200f)
        {
            agent.ChangeState(new WanderState(agent)); // vuelve a explorar
        }
    }
}
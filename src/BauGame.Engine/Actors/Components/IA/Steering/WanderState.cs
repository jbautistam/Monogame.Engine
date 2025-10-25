using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

public class WanderState : SteeringState
{
    public WanderState(Agent agent) : base(agent) { }

    public override void Enter()
    {
        SetBehaviorWeight(new Wander(), 1.0f);
        SetBehaviorWeight(new Seek(() => new Vector2(500, 300)), 0.3f); // Ir a un punto aleatorio
    }

    public override void Update(float deltaTime)
    {
        // Lógica de transición
        if (ShouldFlee())
        {
            agent.ChangeState(new FleeState(agent));
        }
    }

    private bool ShouldFlee()
    {
        // Detectar si hay un enemigo cerca
        // Ejemplo: si hay un punto "enemigo" en Vector2(400, 300) y está cerca
        return Vector2.Distance(agent.Position, new Vector2(400, 300)) < 100f;
    }
}
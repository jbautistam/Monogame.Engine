using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

public class Seek : ISteeringBehavior
{
    private Vector2 target;

    public Seek(Vector2 target) => this.target = target;

    public Vector2 Calculate(Agent agent)
    {
        Vector2 desired = target - agent.Position;
        desired.Normalize();
        desired *= agent.MaxSpeed;
        return desired - agent.Velocity; // fuerza de dirección
    }
}
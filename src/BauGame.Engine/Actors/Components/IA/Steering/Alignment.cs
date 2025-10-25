using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

public class Alignment : ISteeringBehavior
{
    private List<Agent> agents;
    private Agent self;
    private float neighborRadius = 50f;

    public Alignment(List<Agent> agents, Agent self, float neighborRadius = 50f)
    {
        this.agents = agents;
        this.self = self;
        this.neighborRadius = neighborRadius;
    }

    public Vector2 Calculate(Agent agent)
    {
        Vector2 steering = Vector2.Zero;
        int count = 0;

        foreach (var other in agents)
        {
            if (other == self) continue;

            float distance = Vector2.Distance(agent.Position, other.Position);
            if (distance > 0 && distance < neighborRadius)
            {
                steering += other.Velocity;
                count++;
            }
        }

        if (count > 0)
        {
            steering /= count;
            if (steering.Length() > 0)
            {
                steering = Vector2.Normalize(steering) * agent.MaxSpeed;
                steering -= agent.Velocity;
                steering = Vector2.Clamp(steering, -agent.MaxForce, agent.MaxForce);
            }
        }

        return steering;
    }
}
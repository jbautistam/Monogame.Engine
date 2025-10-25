using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

public class Cohesion : ISteeringBehavior
{
    private List<Agent> agents;
    private Agent self;
    private float neighborRadius = 50f;

    public Cohesion(List<Agent> agents, Agent self, float neighborRadius = 50f)
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
                steering += other.Position;
                count++;
            }
        }

        if (count > 0)
        {
            steering /= count; // Centro de masa
            return new Seek(() => steering).Calculate(agent); // Usamos Seek para ir hacia el centro
        }

        return Vector2.Zero;
    }
}
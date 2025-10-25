using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

public class Flock : ISteeringBehavior
{
    private List<Agent> agents;
    private Agent self;

    public Flock(List<Agent> agents, Agent self)
    {
        this.agents = agents;
        this.self = self;
    }

    public Vector2 Calculate(Agent agent)
    {
        Vector2 separation = new Separation(agents, self).Calculate(agent) * 1.5f;
        Vector2 alignment = new Alignment(agents, self).Calculate(agent) * 1.0f;
        Vector2 cohesion = new Cohesion(agents, self).Calculate(agent) * 1.0f;

        return separation + alignment + cohesion;
    }
}
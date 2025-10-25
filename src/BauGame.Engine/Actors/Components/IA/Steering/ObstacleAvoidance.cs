using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

public class ObstacleAvoidance : ISteeringBehavior
{
    private List<Obstacle> obstacles;
    private float avoidDistance = 100f;

    public ObstacleAvoidance(List<Obstacle> obstacles, float avoidDistance = 100f)
    {
        this.obstacles = obstacles;
        this.avoidDistance = avoidDistance;
    }

    public Vector2 Calculate(Agent agent)
    {
        Vector2 force = Vector2.Zero;

        foreach (var obstacle in obstacles)
        {
            float distance = Vector2.Distance(agent.Position, obstacle.Position);
            if (distance < avoidDistance + obstacle.Radius)
            {
                Vector2 direction = agent.Position - obstacle.Position;
                direction.Normalize();
                direction *= (avoidDistance + obstacle.Radius - distance); // Cuanto más cerca, más fuerte
                force += direction;
            }
        }

        return force;
    }
}
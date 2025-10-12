using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles;

// Componente de seguimiento de camino
public class PathFollowingComponent : AbstractParticleNode
{
    public List<Vector2> Waypoints = new List<Vector2>();
    public float Speed = 100f;
    public bool Loop = true;
    private int currentWaypoint = 0;

    public override void Update(Particle particle, float deltaTime)
    {
        if (Waypoints.Count == 0) return;

        Vector2 target = Waypoints[currentWaypoint];
        Vector2 direction = target - particle.Position;
        float distance = direction.Length();

        if (distance < 5f) // Llegamos al waypoint
        {
            currentWaypoint++;
            if (currentWaypoint >= Waypoints.Count)
            {
                if (Loop)
                    currentWaypoint = 0;
                else
                    return;
            }
        }
        else
        {
            direction.Normalize();
            particle.Velocity = direction * Speed;
        }
    }
}

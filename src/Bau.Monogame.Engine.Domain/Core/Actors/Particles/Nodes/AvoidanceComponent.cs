using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles;

// Componente de evitación
public class AvoidanceComponent : AbstractParticleNode
{
    public List<Vector2> Obstacles = new List<Vector2>();
    public float AvoidanceRadius = 50f;
    public float AvoidanceStrength = 100f;

    public override void Update(Particle particle, float deltaTime)
    {
        foreach (var obstacle in Obstacles)
        {
            Vector2 direction = particle.Position - obstacle;
            float distance = direction.Length();
            
            if (distance < AvoidanceRadius && distance > 1f)
            {
                direction.Normalize();
                float force = AvoidanceStrength * (1f - distance / AvoidanceRadius);
                particle.Velocity += direction * force * deltaTime;
            }
        }
    }
}

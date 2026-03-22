using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles;

// Componente de vida basada en distancia
public class DistanceLifeComponent : AbstractParticleNode
{
    public Vector2 Origin;
    public float MaxDistance = 200f;

    public override void Initialize(Particle particle)
    {
        Origin = particle.Position;
    }

    public override void Update(Particle particle, float deltaTime)
    {
        float distance = Vector2.Distance(particle.Position, Origin);
        if (distance > MaxDistance)
        {
            particle.Life = 0; // Matar la partícula
        }
    }
}

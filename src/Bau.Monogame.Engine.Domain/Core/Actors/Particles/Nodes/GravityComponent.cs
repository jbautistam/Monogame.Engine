using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles.Nodes;

public class GravityComponent : AbstractParticleNode
{
    public Vector2 Gravity = new Vector2(0, 200f);

    public override void Update(Particle particle, float deltaTime)
    {
        particle.Velocity += Gravity * deltaTime;
        particle.Position += particle.Velocity * deltaTime;
    }
}

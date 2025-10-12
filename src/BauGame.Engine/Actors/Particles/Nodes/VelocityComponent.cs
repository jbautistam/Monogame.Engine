using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles.Nodes;

public class VelocityComponent : AbstractParticleNode
{
    public Vector2 InitialVelocityMin;
    public Vector2 InitialVelocityMax;
    private Random random = new Random();

    public override void Initialize(Particle particle)
    {
        particle.Velocity = new Vector2(
            MathHelper.Lerp(InitialVelocityMin.X, InitialVelocityMax.X, (float)random.NextDouble()),
            MathHelper.Lerp(InitialVelocityMin.Y, InitialVelocityMax.Y, (float)random.NextDouble())
        );
    }

    public override void Reset(Particle particle)
    {
        particle.Velocity = Vector2.Zero;
    }
}

namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles.Nodes;

public class ScaleInterpolationComponent : AbstractParticleNode
{
    public float StartScale = 1f;
    public float EndScale = 0.1f;

    public override void Initialize(Particle particle)
    {
        particle.StartScale = StartScale;
        particle.EndScale = EndScale;
        particle.Scale = StartScale;
    }

    public override void Update(Particle particle, float deltaTime)
    {
        if (particle.MaxLife > 0)
        {
            float progress = 1f - (particle.Life / particle.MaxLife);
            particle.Scale = MathHelper.Lerp(particle.StartScale, particle.EndScale, progress);
        }
    }

    public override void Reset(Particle particle)
    {
        particle.StartScale = 1f;
        particle.EndScale = 1f;
        particle.Scale = 1f;
    }
}

using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles.Nodes;

public class ColorInterpolationComponent : AbstractParticleNode
{
    public Color StartColor = Color.White;
    public Color EndColor = Color.Transparent;

    public override void Initialize(Particle particle)
    {
        particle.StartColor = StartColor;
        particle.EndColor = EndColor;
        particle.Color = StartColor;
    }

    public override void Update(Particle particle, float deltaTime)
    {
        if (particle.MaxLife > 0)
        {
            float progress = 1f - (particle.Life / particle.MaxLife);
            particle.Color = Color.Lerp(particle.StartColor, particle.EndColor, progress);
        }
    }

    public override void Reset(Particle particle)
    {
        particle.StartColor = Color.White;
        particle.EndColor = Color.White;
        particle.Color = Color.White;
    }
}
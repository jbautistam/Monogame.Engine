using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles;

// Componente de pulsación
public class PulseComponent : AbstractParticleNode
{
    public float PulseSpeed = 2f;
    public float PulseMinScale = 0.5f;
    public float PulseMaxScale = 1.5f;
    private float pulseTimer = 0f;

    public override void Initialize(Particle particle)
    {
        particle.StartScale = (PulseMinScale + PulseMaxScale) / 2f;
        particle.Scale = particle.StartScale;
    }

    public override void Update(Particle particle, float deltaTime)
    {
        pulseTimer += deltaTime * PulseSpeed;
        float pulse = (float)Math.Sin(pulseTimer) * 0.5f + 0.5f;
        particle.Scale = MathHelper.Lerp(PulseMinScale, PulseMaxScale, pulse);
    }

    public override void Reset(Particle particle)
    {
        pulseTimer = 0f;
    }
}

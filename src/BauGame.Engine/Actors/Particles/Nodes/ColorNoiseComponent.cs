using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles;

// Componente de distorsión de color
public class ColorNoiseComponent : AbstractParticleNode
{
    public float NoiseIntensity = 0.1f;
    private Random random = new Random();

    public override void Update(Particle particle, float deltaTime)
    {
        // Añadir ruido al color
        float noise = (float)(random.NextDouble() * 2 - 1) * NoiseIntensity;
        particle.Color = new Color(
            MathHelper.Clamp(particle.Color.R / 255f + noise, 0, 1),
            MathHelper.Clamp(particle.Color.G / 255f + noise, 0, 1),
            MathHelper.Clamp(particle.Color.B / 255f + noise, 0, 1),
            particle.Color.A / 255f
        );
    }
}
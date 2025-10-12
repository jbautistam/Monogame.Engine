using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles;

// Componente de viento
public class WindComponent : AbstractParticleNode
{
    public Vector2 WindForce = new Vector2(50, 0);
    public float Turbulence = 10f;
    private Random random = new Random();

    public override void Update(Particle particle, float deltaTime)
    {
        Vector2 turbulence = new Vector2(
            (float)(random.NextDouble() * 2 - 1) * Turbulence,
            (float)(random.NextDouble() * 2 - 1) * Turbulence
        );
        
        particle.Velocity += (WindForce + turbulence) * deltaTime;
    }
}

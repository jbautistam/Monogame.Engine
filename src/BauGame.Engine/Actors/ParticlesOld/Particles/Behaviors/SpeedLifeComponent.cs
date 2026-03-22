namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles;

// Componente de vida basada en velocidad
public class SpeedLifeComponent : AbstractParticleNode
{
    public float MinSpeed = 10f;
    public float SpeedFadeDuration = 0.5f;

    public override void Update(Particle particle, float deltaTime)
    {
        float speed = particle.Velocity.Length();
        if (speed < MinSpeed)
        {
            // Reducir vida más rápido cuando la velocidad es baja
            particle.Life -= deltaTime * (1f + (MinSpeed - speed) / MinSpeed);
        }
    }
}

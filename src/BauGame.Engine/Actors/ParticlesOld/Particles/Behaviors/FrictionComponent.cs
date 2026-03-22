using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles;

// Componente de fricción
public class FrictionComponent : AbstractParticleNode
{
    public float FrictionCoefficient = 0.02f;

    public override void Update(Particle particle, float deltaTime)
    {
        // Aplicar fricción basada en la velocidad
        float speed = particle.Velocity.Length();
        if (speed > 0)
        {
            Vector2 direction = particle.Velocity / speed;
            float friction = speed * FrictionCoefficient;
            particle.Velocity -= direction * friction * deltaTime;
            
            // Detener si la velocidad es muy baja
            if (particle.Velocity.Length() < 0.1f)
            {
                particle.Velocity = Vector2.Zero;
            }
        }
    }
}

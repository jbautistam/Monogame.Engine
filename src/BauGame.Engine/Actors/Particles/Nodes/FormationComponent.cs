using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles;

// Componente de formación
public class FormationComponent : AbstractParticleNode
{
    public Vector2 FormationCenter;
    public float FormationRadius = 100f;
    public float FormationStrength = 50f;
    private int particleIndex;

    public override void Initialize(Particle particle)
    {
        // Calcular posición en formación circular
        float angle = (particleIndex * 0.1f) % (MathHelper.Pi * 2);
        FormationCenter = particle.Position + new Vector2(
            (float)Math.Cos(angle) * FormationRadius,
            (float)Math.Sin(angle) * FormationRadius
        );
    }

    public override void Update(Particle particle, float deltaTime)
    {
        Vector2 direction = FormationCenter - particle.Position;
        float distance = direction.Length();
        
        if (distance > 1f)
        {
            direction.Normalize();
            particle.Velocity += direction * FormationStrength * deltaTime;
        }
    }
}

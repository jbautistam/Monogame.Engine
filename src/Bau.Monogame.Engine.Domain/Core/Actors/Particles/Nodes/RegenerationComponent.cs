namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles;

// Componente de regeneración
public class RegenerationComponent : AbstractParticleNode
{
    public float RegenRate = 0.1f; // Vida por segundo
    public float MaxRegens = 3; // Número máximo de regeneraciones
    private int regenCount = 0;

    public override void Update(Particle particle, float deltaTime)
    {
        if (regenCount < MaxRegens && particle.Life < particle.MaxLife * 0.3f)
        {
            particle.Life += RegenRate * deltaTime;
            if (particle.Life >= particle.MaxLife)
            {
                regenCount++;
                particle.Life = particle.MaxLife;
            }
        }
    }

    public override void Reset(Particle particle)
    {
        regenCount = 0;
    }
}

namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles;

// Componente de explosión
public class ExplosionComponent : AbstractParticleNode
{
    public float ExplosionDelay = 1f;
    public float ExplosionRadius = 50f;
    public float ExplosionForce = 200f;
    private float timer = 0f;
    private bool hasExploded = false;

    public override void Update(Particle particle, float deltaTime)
    {
        if (hasExploded) return;

        timer += deltaTime;
        if (timer >= ExplosionDelay)
        {
            // Aquí se podría activar una explosión que afecte otras partículas
            hasExploded = true;
        }
    }

    public override void Reset(Particle particle)
    {
        timer = 0f;
        hasExploded = false;
    }
}

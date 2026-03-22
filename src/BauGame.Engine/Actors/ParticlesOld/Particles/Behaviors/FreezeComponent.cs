using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles;

// Componente de congelamiento
public class FreezeComponent : AbstractParticleNode
{
    public float FreezeDelay = 0.5f;
    public float FreezeDuration = 1f;
    private float timer = 0f;
    private bool isFrozen = false;
    private Vector2 frozenVelocity;

    public override void Update(Particle particle, float deltaTime)
    {
        timer += deltaTime;

        if (!isFrozen && timer >= FreezeDelay)
        {
            isFrozen = true;
            frozenVelocity = particle.Velocity;
            particle.Velocity = Vector2.Zero;
        }
        else if (isFrozen && timer >= FreezeDelay + FreezeDuration)
        {
            isFrozen = false;
            particle.Velocity = frozenVelocity;
        }
    }

    public override void Reset(Particle particle)
    {
        timer = 0f;
        isFrozen = false;
    }
}

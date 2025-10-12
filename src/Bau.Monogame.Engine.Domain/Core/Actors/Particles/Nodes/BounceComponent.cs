using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles;

// Componente de rebote
public class BounceComponent : AbstractParticleNode
{
    public Rectangle Bounds;
    public float BounceDamping = 0.8f;
    public bool IsWrapping = false;

    public override void Update(Particle particle, float deltaTime)
    {
        // Rebote en los bordes
        if (particle.Position.X <= Bounds.Left || particle.Position.X >= Bounds.Right)
        {
            particle.Velocity.X *= -BounceDamping;
            particle.Position.X = MathHelper.Clamp(particle.Position.X, Bounds.Left, Bounds.Right);
        }

        if (particle.Position.Y <= Bounds.Top || particle.Position.Y >= Bounds.Bottom)
        {
            particle.Velocity.Y *= -BounceDamping;
            particle.Position.Y = MathHelper.Clamp(particle.Position.Y, Bounds.Top, Bounds.Bottom);
        }

        // Opción de wrapping (teletransporte al otro lado)
        if (IsWrapping)
        {
            if (particle.Position.X < Bounds.Left) particle.Position.X = Bounds.Right;
            if (particle.Position.X > Bounds.Right) particle.Position.X = Bounds.Left;
            if (particle.Position.Y < Bounds.Top) particle.Position.Y = Bounds.Bottom;
            if (particle.Position.Y > Bounds.Bottom) particle.Position.Y = Bounds.Top;
        }
    }
}

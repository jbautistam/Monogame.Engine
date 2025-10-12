namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles;

public abstract class AbstractParticleNode
{
    public virtual void Initialize(Particle particle) { }
    public virtual void Update(Particle particle, float deltaTime) { }
    public virtual void Reset(Particle particle) { }
}

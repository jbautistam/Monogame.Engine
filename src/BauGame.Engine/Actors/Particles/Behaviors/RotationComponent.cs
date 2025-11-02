namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles;

public class RotationComponent : AbstractParticleNode
{
    public float RotationSpeedMin = -2f;
    public float RotationSpeedMax = 2f;
    private Random random = new Random();

    public override void Initialize(Particle particle)
    {
        particle.RotationSpeed = MathHelper.Lerp(RotationSpeedMin, RotationSpeedMax, (float)random.NextDouble());
    }

    public override void Update(Particle particle, float deltaTime)
    {
        particle.Rotation += particle.RotationSpeed * deltaTime;
    }

    public override void Reset(Particle particle)
    {
        particle.Rotation = 0f;
        particle.RotationSpeed = 0f;
    }
}

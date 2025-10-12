namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles;

// Componente de resistencia del aire (drag)
public class DragComponent : AbstractParticleNode
{
    public float DragCoefficient = 0.1f;

    public override void Update(Particle particle, float deltaTime)
    {
        // Aplicar resistencia: reduce la velocidad con el tiempo
        particle.Velocity *= (1f - DragCoefficient * deltaTime);
    }
}

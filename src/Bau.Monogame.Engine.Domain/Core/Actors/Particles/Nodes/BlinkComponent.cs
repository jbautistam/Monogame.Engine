namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles;

// Componente de parpadeo
public class BlinkComponent : AbstractParticleNode
{
    public float BlinkInterval = 0.2f;
    public float BlinkDuration = 0.05f;
    private float blinkTimer = 0f;
    private bool isVisible = true;

    public override void Update(Particle particle, float deltaTime)
    {
        blinkTimer += deltaTime;
        
        if (isVisible && blinkTimer >= BlinkInterval)
        {
            isVisible = false;
            blinkTimer = 0f;
        }
        else if (!isVisible && blinkTimer >= BlinkDuration)
        {
            isVisible = true;
            blinkTimer = 0f;
        }

        // Modificar la transparencia
        if (!isVisible)
        {
            particle.Color = particle.Color * 0.3f; // Más transparente cuando parpadea
        }
    }

    public override void Reset(Particle particle)
    {
        blinkTimer = 0f;
        isVisible = true;
    }
}

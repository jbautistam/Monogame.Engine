namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles;

// Componente de animación de textura (spritesheet)
public class TextureAnimationComponent : AbstractParticleNode
{
    public int FrameCount = 8;
    public float FrameDuration = 0.1f;
    public bool Loop = true;
    private float timer = 0f;

    public override void Update(Particle particle, float deltaTime)
    {
        timer += deltaTime;
        if (timer >= FrameDuration)
        {
            // Aquí iría la lógica para cambiar el frame
            timer = 0f;
        }
    }

    public override void Reset(Particle particle)
    {
        timer = 0f;
    }
}

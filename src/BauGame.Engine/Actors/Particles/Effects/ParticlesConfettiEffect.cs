using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Particles.Effects;

/// <summary>
///		Efecto para emisión de un sistema de confetti
/// </summary>
public class ParticlesConfettiEffect(int numberOfParticles, Color? startColor = null) : AbstracParticlesEffect(numberOfParticles, startColor)
{
    /// <summary>
    ///     Emite las partículas
    /// </summary>
    public override void Emit(ParticlesSystemActor particlesSystem)
    {
        for (int index = 0; index < NumberOfParticles; index++)
        {
            Vector2 direction = new(Tools.Randomizer.GetRandom(-1, 1), Tools.Randomizer.GetRandom(0, 1));
            float speed = Tools.Randomizer.GetRandom(50, 250);
            float lifetime = Tools.Randomizer.GetRandom(1, 4);
            float scale = Tools.Randomizer.GetRandom(0.3f, 0.8f);

                // Normaliza la dirección
                direction.Normalize();
                // Crea la partícula y le asigna una cola
                particlesSystem.CreateParticle(particlesSystem.Position, direction, speed, lifetime, scale, 
                                               StartColor ?? Tools.Randomizer.GetRandomColor());
        }
    }
}
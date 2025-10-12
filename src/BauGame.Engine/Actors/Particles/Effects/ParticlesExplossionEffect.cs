using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Particles.Effects;

/// <summary>
///		Efecto para emisión de un sistema de partículas de una explosión
/// </summary>
public class ParticlesExplossionEffect(int numberOfParticles, Color? startColor = null) : AbstracParticlesEffect(numberOfParticles, startColor)
{
    /// <summary>
    ///     Emite las partículas
    /// </summary>
    public override void Emit(ParticlesSystemActor particlesSystem)
    {
        for (int index = 0; index < NumberOfParticles; index++)
        {
            Vector2 direction = Tools.Randomizer.GetRandomDirection();
            float speed = Tools.Randomizer.GetRandom(100, 400);
            float lifetime = Tools.Randomizer.GetRandom(0.5f, 2);
            float scale = Tools.Randomizer.GetRandom(0.2f, 0.7f);

                // Crea la partícula y le asigna una cola
                particlesSystem.CreateParticle(particlesSystem.Position, direction, speed, lifetime, scale, 
                                               StartColor ?? Tools.Randomizer.GetRandomColor(200, 256, 100, 200, 0, 100))
                        .TailLength = 10;
        }
    }
}

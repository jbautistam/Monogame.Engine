using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles.Effects;

/// <summary>
///		Efecto para emisión de un sistema de partículas para fuegos artificales
/// </summary>
public class ParticlesFireworkEffect(int numberOfParticles, Color? startColor = null) : AbstracParticlesEffect(numberOfParticles, startColor)
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
            float lifetime = Tools.Randomizer.GetRandom(3, 1);
            float scale = Tools.Randomizer.GetRandom(-1, 1);

                // Crea la partícula con los datos especificados
                particlesSystem.CreateParticle(particlesSystem.Position, direction, speed, lifetime, scale, 
                                               StartColor ?? Tools.Randomizer.GetRandomColor());
        }
    }
}
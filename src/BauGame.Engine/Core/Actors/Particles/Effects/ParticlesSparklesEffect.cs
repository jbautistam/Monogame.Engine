using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Core.Actors.Particles.Effects;

/// <summary>
///		Efecto para emisión de un sistema de chispas
/// </summary>
public class ParticlesSparklesEffect(int numberOfParticles, Color? startColor = null) : AbstracParticlesEffect(numberOfParticles, startColor)
{
    /// <summary>
    ///     Emite las partículas
    /// </summary>
    public override void Emit(ParticlesSystemActor particlesSystem)
    {
        for (int index = 0; index < NumberOfParticles; index++)
        {
            Vector2 direction = Tools.Randomizer.GetRandomDirection();
            float speed = Tools.Randomizer.GetRandom(0, 300);
            float lifetime = Tools.Randomizer.GetRandom(0.5f, 1.5f);
            float scale = Tools.Randomizer.GetRandom(0.2f, 0.7f);

                // Crea la partícula
                particlesSystem.CreateParticle(particlesSystem.Position, direction, speed, lifetime, scale, 
                                               StartColor ?? Tools.Randomizer.GetRandomColor());
        }
    }
}
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Actors.ParticlesEngine.Modifiers;

/// <summary>
///     Modificador con efecto de viento
/// </summary>
public class WindModifier(Vector2 direction, float strength, float turbulence = 0f) : AbstractParticleModifier
{
    /// <summary>
    ///     Actualiza los datos de la partícula
    /// </summary>
    protected override void UpdateSelf(ParticleModel particle, float deltaTime, float normalizedAge)
    {
        float turbulenceFactor = Math.Abs((float) (Tools.Randomizer.Random.NextDouble() * 2 - 1) * turbulence);
        Vector2 windForce = Vector2.Normalize(direction) * (strength + turbulenceFactor);

            // Cambia la velocidad de la partícula
            particle.Velocity += windForce * deltaTime;
    }
}
using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.ParticlesEngine.Modifiers;

/// <summary>
///     Modificador de gravedad
/// </summary>
public class GravityModifier(float gravity = 9.8f) : AbstractParticleModifier
{
    /// <summary>
    ///     Actualiza la velocidad de la partícula
    /// </summary>
    protected override void UpdateSelf(ParticleModel particle, float deltaTime, float normalizedAge)
    {
        particle.Velocity += Gravity * deltaTime;
    }

    /// <summary>
    ///     Vector de gravedad
    /// </summary>
    public Vector2 Gravity { get; set; } = new(0, gravity);
}

using Microsoft.Xna.Framework;
using Bau.BauEngine.Actors.ParticlesEngine.Particles;

namespace Bau.BauEngine.Actors.ParticlesEngine.Modifiers;

/// <summary>
///     Modificador de gravedad
/// </summary>
public class GravityModifier(float gravity = 9.8f) : AbstractParticleModifier
{
    /// <summary>
    ///     Clona los datos de un modificador
    /// </summary>
    public override AbstractParticleModifier Clone() => new GravityModifier(gravity);

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

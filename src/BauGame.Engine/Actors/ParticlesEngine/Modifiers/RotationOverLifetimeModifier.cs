using Bau.BauEngine.Actors.ParticlesEngine.Particles;

namespace Bau.BauEngine.Actors.ParticlesEngine.Modifiers;

/// <summary>
///     Modificador de la rotación de la partícula
/// </summary>
public class RotationOverLifetimeModifier(float rotationSpeed) : AbstractParticleModifier
{
    /// <summary>
    ///     Clona los datos de un modificador
    /// </summary>
    public override AbstractParticleModifier Clone() => new RotationOverLifetimeModifier(rotationSpeed);

    /// <summary>
    ///     Actualiza la partícula
    /// </summary>
    protected override void UpdateSelf(ParticleModel particle, float deltaTime, float normalizedAge)
    {
        particle.Rotation += rotationSpeed * deltaTime;
    }
}

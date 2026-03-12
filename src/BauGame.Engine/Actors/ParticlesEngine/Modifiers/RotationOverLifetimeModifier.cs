namespace Bau.Libraries.BauGame.Engine.Actors.ParticlesEngine.Modifiers;

/// <summary>
///     Modificador de la rotación de la partícula
/// </summary>
public class RotationOverLifetimeModifier(float rotationSpeed) : AbstractParticleModifier
{
    /// <summary>
    ///     Actualiza la partícula
    /// </summary>
    protected override void UpdateSelf(ParticleModel particle, float deltaTime, float normalizedAge)
    {
        particle.Rotation += rotationSpeed * deltaTime;
    }
}

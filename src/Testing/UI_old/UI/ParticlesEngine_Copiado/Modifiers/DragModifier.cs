namespace UI.CharactersEngine.ParticlesEngine.Modifiers;

/// <summary>
///     Modfica la velocidad de la partícula dependiendo de la fricción
/// </summary>
public class DragModifier(float dragCoefficient = 0.95f) : AbstractParticleModifier
{
    /// <summary>
    ///     Actualiza la velocidad de la partícula
    /// </summary>
    protected override void UpdateSelf(ParticleModel particle, float deltaTime, float normalizedAge)
    {
        particle.Velocity *= dragCoefficient;
    }
}

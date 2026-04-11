using Bau.BauEngine.Actors.ParticlesEngine.Particles;

namespace Bau.BauEngine.Actors.ParticlesEngine.Modifiers;

/// <summary>
///     Modfica la velocidad de la partícula dependiendo de la fricción
/// </summary>
public class DragModifier(float dragCoefficient = 0.95f) : AbstractParticleModifier
{
    /// <summary>
    ///     Clona los datos de un modificador
    /// </summary>
    public override AbstractParticleModifier Clone() => new DragModifier(dragCoefficient);

    /// <summary>
    ///     Actualiza la velocidad de la partícula
    /// </summary>
    protected override void UpdateSelf(ParticleModel particle, float deltaTime, float normalizedAge)
    {
        particle.Velocity *= dragCoefficient;
    }
}

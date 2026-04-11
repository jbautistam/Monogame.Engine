using Bau.BauEngine.Tools.MathTools.Easing;
using Bau.BauEngine.Actors.ParticlesEngine.Particles;

namespace Bau.BauEngine.Actors.ParticlesEngine.Modifiers;

/// <summary>
///     Modificador de la escala a partir del tiempo de vida de la partícula
/// </summary>
public class ScaleOverLifetimeModifier(float minimum, float maximum, 
                                       EasingFunctionsHelper.EasingType easingType = EasingFunctionsHelper.EasingType.Linear) : AbstractParticleModifier
{
    /// <summary>
    ///     Clona los datos de un modificador
    /// </summary>
    public override AbstractParticleModifier Clone() => new ScaleOverLifetimeModifier(minimum, maximum, easingType);

    /// <summary>
    ///     Actualiza los datos de la partícula
    /// </summary>
    protected override void UpdateSelf(ParticleModel particle, float deltaTime, float normalizedAge)
    {
        if (particle.Scale >= minimum && particle.Scale < maximum)
            particle.Scale = EasingFunctionsHelper.Interpolate(particle.Scale, maximum, normalizedAge, easingType);
    }
}
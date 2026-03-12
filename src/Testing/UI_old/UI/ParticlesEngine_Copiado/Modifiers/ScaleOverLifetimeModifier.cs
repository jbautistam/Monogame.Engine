using UI.CharactersEngine.MathTools.Easing;

namespace UI.CharactersEngine.ParticlesEngine.Modifiers;

/// <summary>
///     Modificador de la escala a partir del tiempo de vida de la partícula
/// </summary>
public class ScaleOverLifetimeModifier(float minimum, float maximum, 
                                       EasingFunctionsHelper.EasingType easingType = EasingFunctionsHelper.EasingType.Linear) : AbstractParticleModifier
{
    /// <summary>
    ///     Actualiza los datos de la partícula
    /// </summary>
    protected override void UpdateSelf(ParticleModel particle, float deltaTime, float normalizedAge)
    {
        if (particle.Scale >= minimum && particle.Scale < maximum)
            particle.Scale = EasingFunctionsHelper.Interpolate(particle.Scale, maximum, normalizedAge, easingType);
    }
}
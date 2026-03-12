using Microsoft.Xna.Framework;
using UI.CharactersEngine.MathTools.Easing;

namespace UI.CharactersEngine.ParticlesEngine.Modifiers;

/// <summary>
///     Modificador del tiempo del alfa de la partícula a partir del tiempo de vida
/// </summary>
public class AlphaOverLifetimeModifier(int minimum, int maximum, 
                                       EasingFunctionsHelper.EasingType easingType = EasingFunctionsHelper.EasingType.Linear) : AbstractParticleModifier
{
    /// <summary>
    ///     Actualiza la transparencia de la partícula
    /// </summary>
    protected override void UpdateSelf(ParticleModel particle, float deltaTime, float normalizedAge)
    {
        if (particle.Color.A > minimum && particle.Color.A < maximum)
        {
            float alpha = EasingFunctionsHelper.Interpolate(particle.Color.A, maximum, normalizedAge, easingType);

                // Cambia la transparencia del color
                particle.Color = new Color(particle.Color.R, particle.Color.G, particle.Color.B, (byte) (alpha * 255));
        }
    }
}

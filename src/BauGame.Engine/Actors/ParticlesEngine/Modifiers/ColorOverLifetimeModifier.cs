using Bau.Libraries.BauGame.Engine.Tools.MathTools.Easing;

using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.ParticlesEngine.Modifiers;

/// <summary>
///     Modificador del color de la partícula a lo largo del tiempo
/// </summary>
public class ColorOverLifetimeModifier(EasingFunctionsHelper.EasingType easingType = EasingFunctionsHelper.EasingType.Linear) : AbstractParticleModifier
{
    /// <summary>
    ///     Actualiza la partícula
    /// </summary>
    protected override void UpdateSelf(ParticleModel particle, float deltaTime, float normalizedAge)
    {
        if (Colors.Count > 0)
            particle.Color = EvaluateGradient(normalizedAge);
    }

    /// <summary>
    ///     Busca el color adecuado
    /// </summary>
    private Color EvaluateGradient(float normalizedAge)
    {
        // Ordena los colores
        Colors.Sort((first, second) => first.Age.CompareTo(second.Age));
        // Obtiene el color interpolando
        if (Colors.Count == 1) 
            return Colors[0].Color;
        else
            for (int index = 0; index < Colors.Count - 1; index++)
                if (normalizedAge >= Colors[index].Age)
                    return EasingFunctionsHelper.Interpolate(Colors[index].Color, Colors[index + 1].Color, normalizedAge, easingType);
        // Si ha llegado hasta aquí, devuelve el último
        return Colors[Colors.Count - 1].Color;
    }

    /// <summary>
    ///     Lista de colores que va a tomar la partícula a lo largo del tiempo
    /// </summary>
    public List<(float Age, Color Color)> Colors { get; } = [];
}
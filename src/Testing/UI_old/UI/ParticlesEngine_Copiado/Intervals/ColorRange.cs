using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.ParticlesEngine.Intervals;

/// <summary>
///     Rango de colores
/// </summary>
public readonly record struct ColorRange(Color Min, Color Max)
{
    /// <summary>
    ///     Obtiene un valor aleatorio entre dos colores
    /// </summary>
    public Color GetValue(Random random)
    {
        if (Min == Max)
            return Min;
        else
            return Color.Lerp(Min, Max, (float) random.NextDouble());
    }
}

using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Tools.MathTools.Intervals;

/// <summary>
///     Rango de colores
/// </summary>
public readonly record struct ColorRange(Color Min, Color Max)
{
    /// <summary>
    ///     Obtiene un valor aleatorio entre dos colores
    /// </summary>
    public Color GetValue()
    {
        if (Min == Max)
            return Min;
        else
            return Color.Lerp(Min, Max, (float) Randomizer.Random.NextDouble());
    }
}

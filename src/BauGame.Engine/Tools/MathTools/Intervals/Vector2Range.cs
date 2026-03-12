using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Tools.MathTools.Intervals;

/// <summary>
///     Rango entre dos vectores
/// </summary>
public readonly record struct Vector2Range(Vector2 Min, Vector2 Max)
{
    /// <summary>
    ///     Obtiene un valor aleatorio del rango
    /// </summary>
    public Vector2 GetValue(Random random)
    {
        if (Min == Max)
            return Min;
        else
            return Randomizer.GetRandomVector(Min, Max);
    }
}

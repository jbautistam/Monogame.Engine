using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.ParticlesEngine.Intervals;

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
            return new Vector2(Min.X + (float) (random.NextDouble() * (Max.X - Min.X)), Min.Y + (float) (random.NextDouble() * (Max.Y - Min.Y)));
    }
}

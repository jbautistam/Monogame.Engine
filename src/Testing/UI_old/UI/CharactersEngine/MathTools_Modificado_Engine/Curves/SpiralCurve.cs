using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.MathTools.Interpolation.Curves;

/// <summary>
///     Espiral de centro a esquina
/// </summary>
public class SpiralCurve(int turns) : AbstractCurve
{
    /// <summary>
    ///     Evalúa la curva
    /// </summary>
    public override Vector2 Evaluate(float t)
    {
        float angle = t * Turns * MathHelper.TwoPi;
        float radius = t * 0.5f; // De 0 a 0.5 (para caber en 0-1)
        
            // Evalúa el vector
            return new Vector2(0.5f + MathF.Cos(angle) * radius, 0.5f + MathF.Sin(angle) * radius);
    }

    /// <summary>
    ///     Número de vueltas
    /// </summary>
    public float Turns { get; } = turns;
}

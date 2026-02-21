using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Tools.MathTools.Curves;

/// <summary>
///     Curva Bézier cuadrática 
///     p0=(0,0), p2=(1,0), p1=(0.5, h) donde h es la "altura" de la curva
/// </summary>
public class QuadraticBezierCurve(float height = 0.5f) : AbstractCurve
{
    /// <summary>
    ///     Evalúa la curva
    /// </summary>
    public override Vector2 Evaluate(float t)
    {
        float u = 1 - t;
        float x = u * u * 0 + 2 * u * t * 0.5f + t * t * 1; // De 0 a 1
        float y = u * u * 0 + 2 * u * t * Height + t * t * 0; // Arco y vuelve a 0

            // Devuelve el vector
            return new Vector2(x, y);
    }

    /// <summary>
    ///     Altura de la curva
    /// </summary>
    public float Height { get; } = height;
}

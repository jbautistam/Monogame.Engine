using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Tools.MathTools.Curves;

/// <summary>
/// Bézier cúbica con control de entrada/salida
///     p0=(0,0), p3=(1,0)
///     p1=(tensionIn, heightIn), p2=(1-tensionOut, heightOut)
/// </summary>
public class CubicBezierCurve(float tensionIn = 0.3f, float heightIn = 0.5f, float tensionOut = 0.3f, float heightOut = -0.5f) : AbstractCurve
{
    /// <summary>
    ///     Evalúa la curva en un momento del tiempo
    /// </summary>
    public override Vector2 Evaluate(float t)
    {
        Vector2 p0 = new(0, 0);
        Vector2 p1 = new(TensionIn, HeightIn);
        Vector2 p2 = new(1 - TensionOut, HeightOut);
        Vector2 p3 = new(1, 0);        
        float u = 1 - t;
        float u2 = u * u;
        float u3 = u2 * u;
        float t2 = t * t;
        float t3 = t2 * t;
        
            // Devuelve el valor de la curva
            return u3 * p0 + 3 * u2 * t * p1 + 3 * u * t2 * p2 + t3 * p3;
    }

    /// <summary>
    ///     0-1, cuánto se acerca al control de entrada
    /// </summary>
    public float TensionIn { get; } = tensionIn;

    /// <summary>
    ///     Altura del control de entrada
    /// </summary>
    public float HeightIn { get; } = heightIn;

    /// <summary>
    ///     0-1, cuánto se acerca al control de salida  
    /// </summary>
    public float TensionOut { get; } = tensionOut;

    /// <summary>
    ///     Altura del control de salida
    /// </summary>
    public float HeightOut { get; } = heightOut;
}

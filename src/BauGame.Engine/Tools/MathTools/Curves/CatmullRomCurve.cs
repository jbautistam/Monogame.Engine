using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Tools.MathTools.Curves;

/// <summary>
///     Spline Catmull-Rom con puntos en UV
/// </summary>
public class CatmullRomCurve(List<Vector2> points) : AbstractCurve
{
    /// <summary>
    ///     Obtiene el punto en el tiempo
    /// </summary>
    public override Vector2 Evaluate(float t)
    {
        int segments = Points.Count - 1;
        float scaledT = t * segments;
        int i = MathHelper.Clamp((int) MathF.Floor(scaledT), 0, segments - 1);
        float localT = scaledT - i;
        Vector2 p0 = Points[Math.Max(0, i - 1)];
        Vector2 p1 = Points[i];
        Vector2 p2 = Points[Math.Min(Points.Count - 1, i + 1)];
        Vector2 p3 = Points[Math.Min(Points.Count - 1, i + 2)];
        float t2 = localT * localT;
        float t3 = t2 * localT;
        
            // Devuelve el vector
            return 0.5f * ((2 * p1) + (-p0 + p2) * localT + (2 * p0 - 5 * p1 + 4 * p2 - p3) * t2 + (-p0 + 3 * p1 - 3 * p2 + p3) * t3);
    }

    /// <summary>
    ///     Puntos de la curva
    /// </summary>
    public List<Vector2> Points { get; set; } = points;
}
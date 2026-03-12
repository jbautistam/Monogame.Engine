using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.MathTools.Interpolation.Curves;

/// <summary>
///     Curva de un arco semicircular normalizado
/// </summary>
public class ArcCurve(float height) : AbstractCurve
{
    /// <summary>
    ///     Evalúa el punto en la curva
    /// </summary>
    public override Vector2 Evaluate(float t) => new Vector2(t, MathF.Sin(t * MathF.PI) * Height);

    /// <summary>
    ///     Altura (0.5 = hasta el medio del círculo, 1: el punto más alto
    /// </summary>
    public float Height { get; } = height;
}

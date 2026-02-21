using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Tools.MathTools.Curves;

/// <summary>
///     Línea recta normalizada: (0,0) a (1,0) horizontal
/// </summary>
public class LinearCurve : AbstractCurve
{
    /// <summary>
    ///     Evalúa la curva
    /// </summary>
    public override Vector2 Evaluate(float t) => new(t, 0);
}

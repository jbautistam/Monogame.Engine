using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Tools.MathTools.Curves;

/// <summary>
///     Clase abstracta para la evaluación de curvas
/// </summary>
public abstract class AbstractCurve
{
    /// <summary>
    ///     Evalúa la curva en un instante
    /// </summary>
    public abstract Vector2 Evaluate(float t);
}

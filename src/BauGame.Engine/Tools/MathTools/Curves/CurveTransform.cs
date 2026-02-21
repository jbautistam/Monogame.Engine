using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Tools.MathTools.Curves;

/// <summary>
///     Transformación de curva: escala, rotación, traslación
/// </summary>
public class CurveTransform
{
    /// <summary>
    ///     Aplica la transformación a un punto UV
    /// </summary>
    public Vector2 Transform(Vector2 uv)
    {
        Vector2 scaled = uv * Scale;
        float cos = MathF.Cos(Rotation);
        float sin = MathF.Sin(Rotation);
        Vector2 rotated = new Vector2(scaled.X * cos - scaled.Y * sin,scaled.X * sin + scaled.Y * cos);
        
            // Interpola entre inicio y final según u. Para curvas que van de start a end:
            return Vector2.Lerp(Start, End, uv.X) + new Vector2(0, rotated.Y);
    }
    
    /// <summary>
    ///     Transforma la curva con un offset
    /// </summary>
    public Vector2 TransformFree(Vector2 uv)
    {
        Vector2 scaled = uv * Scale;
        float cos = MathF.Cos(Rotation);
        float sin = MathF.Sin(Rotation);

            // Devuelve el vector
            return new Vector2(scaled.X * cos - scaled.Y * sin + Start.X, scaled.X * sin + scaled.Y * cos + Start.Y);
    }

    /// <summary>
    ///     Punto inicial en mundo
    /// </summary>
    public Vector2 Start { get; set; }

    /// <summary>
    ///     Punto final en mundo (para curvas que conectan)
    /// </summary>
    public Vector2 End { get; set; }

    /// <summary>
    ///     Escala global
    /// </summary>
    public float Scale { get; set; } = 1f;

    /// <summary>
    ///     Rotación en radianes
    /// </summary>
    public float Rotation { get; set; }
}
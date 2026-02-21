using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.MathTools.Interpolation.Curves;

// Transformación de curva: escala, rotación, traslación
public class CurveTransform
{
    public Vector2 Start { get; set; }      // Punto inicial en mundo
    public Vector2 End { get; set; }        // Punto final en mundo (para curvas que conectan)
    public float Scale { get; set; } = 1f;  // Escala global
    public float Rotation { get; set; }     // Rotación en radianes
    
    // Aplica transformación a punto UV
    public Vector2 Transform(Vector2 uv)
    {
        // Escalar
        var scaled = uv * Scale;
        
        // Rotar
        float cos = MathF.Cos(Rotation);
        float sin = MathF.Sin(Rotation);
        var rotated = new Vector2(
            scaled.X * cos - scaled.Y * sin,
            scaled.X * sin + scaled.Y * cos
        );
        
        // Trasladar (interpolar entre start y end según u)
        // Para curvas que van de start a end:
        return Vector2.Lerp(Start, End, uv.X) + new Vector2(0, rotated.Y);
    }
    
    // Alternativa: curva libre con offset
    public Vector2 TransformFree(Vector2 uv)
    {
        var scaled = uv * Scale;
        float cos = MathF.Cos(Rotation);
        float sin = MathF.Sin(Rotation);
        return new Vector2(
            scaled.X * cos - scaled.Y * sin + Start.X,
            scaled.X * sin + scaled.Y * cos + Start.Y
        );
    }
}
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Tools.MathTools.Curves;

public class Keyframe
{
    public float Time { get; }           // 0-1 en la curva total
    public Vector2 Position { get; }     // Posición UV (0-1)
    public Vector2? TangentIn { get; }   // Tangente entrante (null = automática)
    public Vector2? TangentOut { get; }  // Tangente saliente (null = automática)
    public Easing.EasingFunctionsHelper.EasingType? Easing { get; } // Interpolación hasta siguiente keyframe
    
    public Keyframe(float time, Vector2 position, 
        Vector2? tangentIn = null, 
        Vector2? tangentOut = null,
        Easing.EasingFunctionsHelper.EasingType? easing = null)
    {
        Time = MathHelper.Clamp(time, 0f, 1f);
        Position = position;
        TangentIn = tangentIn;
        TangentOut = tangentOut;
        Easing = easing;
    }
    
    // Keyframe con ángulo de tangente en grados
    public static Keyframe WithAngle(float time, Vector2 position, 
        float? angleInDegrees = null, 
        float? angleOutDegrees = null,
        float tangentLength = 0.3f,
        Easing.EasingFunctionsHelper.EasingType? easing = null)
    {
        Vector2? tin = null, tout = null;
        
        if (angleInDegrees.HasValue)
        {
            float rad = MathHelper.ToRadians(angleInDegrees.Value);
            tin = new Vector2(MathF.Cos(rad), MathF.Sin(rad)) * tangentLength;
        }
        
        if (angleOutDegrees.HasValue)
        {
            float rad = MathHelper.ToRadians(angleOutDegrees.Value);
            tout = new Vector2(MathF.Cos(rad), MathF.Sin(rad)) * tangentLength;
        }
        
        return new Keyframe(time, position, tin, tout, easing);
    }
}

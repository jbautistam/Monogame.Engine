using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Managers.Audio.AudioSystems;

// Curva de animación básica para MonoGame
public class SimpleCurve
{
    public enum CurveType { Linear, EaseIn, EaseOut, EaseInOut, Constant }
    
    private CurveType _type;
    
    public SimpleCurve(CurveType type = CurveType.Linear)
    {
        _type = type;
    }
    
    public float Evaluate(float t)
    {
        t = MathHelper.Clamp(t, 0f, 1f);
        
        return _type switch
        {
            CurveType.Linear => t,
            CurveType.EaseIn => t * t,
            CurveType.EaseOut => 1f - (1f - t) * (1f - t),
            CurveType.EaseInOut => t < 0.5f ? 2f * t * t : 1f - MathF.Pow(-2f * t + 2f, 2f) / 2f,
            CurveType.Constant => t >= 1f ? 1f : 0f,
            _ => t
        };
    }
    
    // Evaluar con rangos de entrada/salida personalizados
    public float Evaluate(float value, float inputMin, float inputMax, 
        float outputMin, float outputMax)
    {
        float t = (value - inputMin) / (inputMax - inputMin);
        float evaluated = Evaluate(t);
        return outputMin + evaluated * (outputMax - outputMin);
    }
}
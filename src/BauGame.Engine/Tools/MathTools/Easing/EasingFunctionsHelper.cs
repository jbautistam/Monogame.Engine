using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Tools.MathTools.Easing;

/// <summary>
///     Funciones de easing
/// </summary>
public static class EasingFunctionsHelper
{
    /// <summary>
    ///     Tipos de funciones de suavizado
    /// </summary>
    public enum EasingType
    {
        Linear,
        QuadIn, QuadOut, QuadInOut,
        CubicIn, CubicOut, CubicInOut,
        QuartIn, QuartOut, QuartInOut,
        QuintIn, QuintOut, QuintInOut,
        SineIn, SineOut, SineInOut,
        CircIn, CircOut, CircInOut,
        ExpoIn, ExpoOut, ExpoInOut,
        BackIn, BackOut, BackInOut,
        ElasticIn, ElasticOut, ElasticInOut,
        BounceOut, BounceIn, BounceInOut,
        SmoothStep, SmootherStep,
        Flash,
        Anticipate
    }

    /// <summary>
    ///     Obtiene la función de suavizado a partir del tipo
    /// </summary>
    public static Func<float, float> GetEasingFunction(EasingType type)
    {
        return type switch
                {
                    EasingType.Linear => EasingFunctions.Linear,
                    EasingType.QuadIn => EasingFunctions.QuadIn,
                    EasingType.QuadOut => EasingFunctions.QuadOut,
                    EasingType.QuadInOut => EasingFunctions.QuadInOut,
                    EasingType.CubicIn => EasingFunctions.CubicIn,
                    EasingType.CubicOut => EasingFunctions.CubicOut,
                    EasingType.CubicInOut => EasingFunctions.CubicInOut,
                    EasingType.QuartIn => EasingFunctions.QuartIn,
                    EasingType.QuartOut => EasingFunctions.QuartOut,
                    EasingType.QuartInOut => EasingFunctions.QuartInOut,
                    EasingType.QuintIn => EasingFunctions.QuintIn,
                    EasingType.QuintOut => EasingFunctions.QuintOut,
                    EasingType.QuintInOut => EasingFunctions.QuintInOut,
                    EasingType.SineIn => EasingFunctions.SineIn,
                    EasingType.SineOut => EasingFunctions.SineOut,
                    EasingType.SineInOut => EasingFunctions.SineInOut,
                    EasingType.CircIn => EasingFunctions.CircIn,
                    EasingType.CircOut => EasingFunctions.CircOut,
                    EasingType.CircInOut => EasingFunctions.CircInOut,
                    EasingType.ExpoIn => EasingFunctions.ExpoIn,
                    EasingType.ExpoOut => EasingFunctions.ExpoOut,
                    EasingType.ExpoInOut => EasingFunctions.ExpoInOut,
                    EasingType.BackIn => EasingFunctions.BackIn,
                    EasingType.BackOut => EasingFunctions.BackOut,
                    EasingType.BackInOut => EasingFunctions.BackInOut,
                    EasingType.ElasticIn => EasingFunctions.ElasticIn,
                    EasingType.ElasticOut => EasingFunctions.ElasticOut,
                    EasingType.ElasticInOut => EasingFunctions.ElasticInOut,
                    EasingType.BounceOut => EasingFunctions.BounceOut,
                    EasingType.BounceIn => EasingFunctions.BounceIn,
                    EasingType.BounceInOut => EasingFunctions.BounceInOut,
                    EasingType.SmoothStep => EasingFunctions.SmoothStep,
                    EasingType.SmootherStep => EasingFunctions.SmootherStep,
                    EasingType.Flash => EasingFunctions.Flash,
                    EasingType.Anticipate => EasingFunctions.Anticipate,
                    _ => EasingFunctions.Linear
                };
    }

    /// <summary>
    ///     Interpola dos valores decimales utilizando una función de suavizado
    /// </summary>
    public static float Apply(float t, EasingType type) => GetEasingFunction(type)(t);

    /// <summary>
    ///     Interpola dos valores decimales utilizando una función de suavizado
    /// </summary>
    public static float Interpolate(float from, float to, float t, EasingType type) => Interpolate(from, to, t, GetEasingFunction(type));

    /// <summary>
    ///     Interpola dos valores decimales utilizando una función de suavizado
    /// </summary>
    public static float Interpolate(float from, float to, float t, Func<float, float> easing) => MathHelper.Lerp(from, to, easing(t));

    /// <summary>
    ///     Interpola dos vectores utilizando una función de suavizado
    /// </summary>
    public static Vector2 Interpolate(Vector2 from, Vector2 to, float t, EasingType type) => Interpolate(from, to, t, GetEasingFunction(type));

    /// <summary>
    ///     Interpola dos vectores utilizando una función de suavizado
    /// </summary>
    public static Vector2 Interpolate(Vector2 from, Vector2 to, float t, Func<float, float> easing) => Vector2.Lerp(from, to, easing(t));

    /// <summary>
    ///     Interpola dos vectores utilizando una función de suavizado con una curva cuadrática de Bezier
    /// </summary>
    public static Vector2 BezierInterpolate(Vector2 from, Vector2 to, Vector2 controlPoint, float t, EasingType type)
    {
        return BezierInterpolate(from, to, controlPoint, t, GetEasingFunction(type));
    }

    /// <summary>
    ///     Interpola dos vectores utilizando una función de suavizado con una curva cuadrática de Bezier
    /// </summary>
    public static Vector2 BezierInterpolate(Vector2 from, Vector2 to, Vector2 controlPoint, float t, Func<float, float> easing)
    {
        float easedT = easing(t);
        float u = 1 - easedT;

            // Devuelve el valor para una curva cuadrática de Bezier
            return u * u * from + 2 * u * easedT * controlPoint + easedT * easedT * to;
    }

    /// <summary>
    ///     Interpola dos vectores con rebote
    /// </summary>
    public static Vector2 OvershootInterpolate(Vector2 from, Vector2 to, float overshoot, float t, EasingType type)
    {
        return OvershootInterpolate(from, to, overshoot, t, GetEasingFunction(type));
    }

    /// <summary>
    ///     Interpola dos vectores con rebote
    /// </summary>
    public static Vector2 OvershootInterpolate(Vector2 from, Vector2 to, float overshoot, float t, Func<float, float> easing)
    {
        float easedT = easing(t);
        
            // Si se pasa del valor objetivo, se rebota
            if (easedT < 1f)
            {
                float overshootT = easedT * (1 + overshoot);

                    // Rebote
                    if (overshootT > 1f) 
                        overshootT = 2 - overshootT; 
                    // Interpola el valor
                    return Vector2.Lerp(from, to, overshootT);
            }
            else
                return to;
    }

    /// <summary>
    ///     Interpola dos colores utilizando una función de suavizado
    /// </summary>
    public static Color Interpolate(Color from, Color to, float t, EasingType type) => Interpolate(from, to, t, GetEasingFunction(type));

    /// <summary>
    ///     Interpola dos colores utilizando una función de suavizado
    /// </summary>
    public static Color Interpolate(Color from, Color to, float t, Func<float, float> easing) => Color.Lerp(from, to, easing(t));
}
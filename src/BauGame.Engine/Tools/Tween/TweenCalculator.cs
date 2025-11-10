using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Tools.Tween;

/// <summary>
///     Funciones de cálculo de valores tween
/// </summary>
public static class TweenCalculator
{
    /// <summary>
    ///     Tipos de easing predefinidos
    /// </summary>
    public enum EaseType
    {
        Linear,
        QuadIn,
        QuadOut,
        QuadInOut,
        CubicIn,
        CubicOut,
        CubicInOut,
        QuartIn,
        QuartOut,
        QuartInOut,
        QuintIn,
        QuintOut,
        QuintInOut,
        SineIn,
        SineOut,
        SineInOut,
        ExpoIn,
        ExpoOut,
        ExpoInOut,
        CircIn,
        CircOut,
        CircInOut,
        BackIn,
        BackOut,
        BackInOut,
        ElasticIn,
        ElasticOut,
        ElasticInOut,
        BounceIn,
        BounceOut,
        BounceInOut
    }

    /// <summary>
    ///     Calcula un valor interpolado de float con una función predefinida
    /// </summary>
    public static TweenResult<float> CalculateFloat(float elapsedTime, float duration, float from, float to, EaseType easeType = EaseType.Linear)
    {
        return CalculateFloat(elapsedTime, duration, from, to, GetEaseFunction(easeType));
    }

    /// <summary>
    ///     Calcula un valor interpolado para un float
    /// </summary>
    public static TweenResult<float> CalculateFloat(float elapsedTime, float duration, float from, float to, Func<float, float> easeFunction)
    {
        TweenResult<float> result = Prepare(elapsedTime, duration, to);

            // Calcula el valor si no se ha llegado al final
            if (!result.IsComplete)
            {
                float amount = easeFunction(result.Progress);

                    // Obtiene el valor intermedio
                    result = new TweenResult<float>(MathHelper.Lerp(from, to, amount), result.Progress);
            }
            // Devuelve el resultado
            return result;
    }

    /// <summary>
    ///     Calcula un tween de Vector2 con tipo de easing predefinido
    /// </summary>
    public static TweenResult<Vector2> CalculateVector2(float elapsedTime, float duration, Vector2 from, Vector2 to, EaseType easeType = EaseType.Linear)
    {
        return CalculateVector2(elapsedTime, duration, from, to, GetEaseFunction(easeType));
    }

    /// <summary>
    ///     Calcula el valor interpolado para un Vector2
    /// </summary>
    public static TweenResult<Vector2> CalculateVector2(float elapsedTime, float duration, Vector2 from, Vector2 to, Func<float, float> easeFunction)
    {
        TweenResult<Vector2> result = Prepare(elapsedTime, duration, to);
            
            // Calcula el valor si no se ha llegado al final
            if (!result.IsComplete)
            {
                float amount = easeFunction(result.Progress);

                    // Obtiene el valor intermedio
                    result = new TweenResult<Vector2>(Vector2.Lerp(from, to, amount), result.Progress);
            }
            // Devuelve el resultado
            return result;
    }

    /// <summary>
    ///     Calcula el valor interpolado para un Vector3
    /// </summary>
    public static TweenResult<Vector3> CalculateVector3(float elapsedTime, float duration, Vector3 from, Vector3 to, Func<float, float> easeFunction)
    {
        TweenResult<Vector3> result = Prepare(elapsedTime, duration, to);
            
            // Calcula el valor si no se ha llegado al final
            if (!result.IsComplete)
            {
                float amount = easeFunction(result.Progress);

                    // Obtiene el valor intermedio
                    result = new TweenResult<Vector3>(Vector3.Lerp(from, to, amount), result.Progress);
            }
            // Devuelve el resultado
            return result;
    }

    /// <summary>
    /// Calcula el valor interpolado para un Color
    /// </summary>
    public static TweenResult<Color> CalculateColor(float elapsedTime, float duration, Color from, Color to, Func<float, float> easeFunction)
    {
        TweenResult<Color> result = Prepare(elapsedTime, duration, to);
            
            // Calcula el valor si no se ha llegado al final
            if (!result.IsComplete)
            {
                float amount = easeFunction(result.Progress);

                    // Obtiene el valor intermedio
                    result = new TweenResult<Color>(Color.Lerp(from, to, amount), result.Progress);
            }
            // Devuelve el resultado
            return result;
    }

    /// <summary>
    ///     Calcula el valor interpolado para un Rectangle
    /// </summary>
    public static TweenResult<Rectangle> CalculateRectangle(float elapsedTime, float duration, Rectangle from, Rectangle to, Func<float, float> easeFunction)
    {
        TweenResult<Rectangle> result = Prepare(elapsedTime, duration, to);
            
            // Calcula el valor si no se ha llegado al final
            if (!result.IsComplete)
            {
                float amount = easeFunction(result.Progress);

                    // Obtiene el valor intermedio
                    result = new TweenResult<Rectangle>(new Rectangle((int) MathHelper.Lerp(from.X, to.X, amount), 
                                                                      (int) MathHelper.Lerp(from.Y, to.Y, amount), 
                                                                      (int) MathHelper.Lerp(from.Width, to.Width, amount), 
                                                                      (int) MathHelper.Lerp(from.Height, to.Height, amount)
                                                                     ), 
                                                        result.Progress);
            }
            // Devuelve el resultado
            return result;
    }

    /// <summary>
    ///     Prepara el resultado de la función Tween calculando el progreso
    /// </summary>
    private static TweenResult<TypeData> Prepare<TypeData>(float elapsedTime, float duration, TypeData to)
    {
        if (duration <= 0)
            return EndResult(to);
        else
        {
            float progress = MathHelper.Clamp(elapsedTime / duration, 0f, 1f);
            
                if (progress >= 1)
                    return EndResult(to);
                else
                    return ProgressResult(to, progress);
        }
    }

    /// <summary>
    ///     Obtiene el resultado final
    /// </summary>
    private static TweenResult<TypeData> EndResult<TypeData>(TypeData to) => new TweenResult<TypeData>(to, 1.0f);

    /// <summary>
    ///     Obtiene un resultado intermedido
    /// </summary>
    private static TweenResult<TypeData> ProgressResult<TypeData>(TypeData to, float progress) => new TweenResult<TypeData>(to, progress);

    /// <summary>
    ///     Obtiene la función de easing correspondiente al tipo
    /// </summary>
    private static Func<float, float> GetEaseFunction(EaseType easeType)
    {
        return easeType switch
                    {
                        EaseType.Linear => Linear,
                        EaseType.QuadIn => QuadIn,
                        EaseType.QuadOut => QuadOut,
                        EaseType.QuadInOut => QuadInOut,
                        EaseType.CubicIn => CubicIn,
                        EaseType.CubicOut => CubicOut,
                        EaseType.CubicInOut => CubicInOut,
                        EaseType.QuartIn => QuartIn,
                        EaseType.QuartOut => QuartOut,
                        EaseType.QuartInOut => QuartInOut,
                        EaseType.QuintIn => QuintIn,
                        EaseType.QuintOut => QuintOut,
                        EaseType.QuintInOut => QuintInOut,
                        EaseType.SineIn => SineIn,
                        EaseType.SineOut => SineOut,
                        EaseType.SineInOut => SineInOut,
                        EaseType.ExpoIn => ExpoIn,
                        EaseType.ExpoOut => ExpoOut,
                        EaseType.ExpoInOut => ExpoInOut,
                        EaseType.CircIn => CircIn,
                        EaseType.CircOut => CircOut,
                        EaseType.CircInOut => CircInOut,
                        EaseType.BackIn => BackIn,
                        EaseType.BackOut => BackOut,
                        EaseType.BackInOut => BackInOut,
                        EaseType.ElasticIn => ElasticIn,
                        EaseType.ElasticOut => ElasticOut,
                        EaseType.ElasticInOut => ElasticInOut,
                        EaseType.BounceIn => BounceIn,
                        EaseType.BounceOut => BounceOut,
                        EaseType.BounceInOut => BounceInOut,
                        _ => Linear
                    };
    }

    private static float Linear(float t) => t;

    private static float QuadIn(float t) => t * t;

    private static float QuadOut(float t) => 1 - (1 - t) * (1 - t);

    private static float QuadInOut(float t) => t < 0.5f ? 2 * t * t : 1 - (float) Math.Pow(-2 * t + 2, 2) / 2;

    private static float CubicIn(float t) => t * t * t;

    private static float CubicOut(float t) => 1 - (float) Math.Pow(1 - t, 3);

    private static float CubicInOut(float t) => t < 0.5f ? 4 * t * t * t : 1 - (float) Math.Pow(-2 * t + 2, 3) / 2;

    private static float QuartIn(float t) => t * t * t * t;

    private static float QuartOut(float t) => 1 - (float)Math.Pow(1 - t, 4);

    private static float QuartInOut(float t) => t < 0.5f ? 8 * t * t * t * t : 1 - (float) Math.Pow(-2 * t + 2, 4) / 2;

    private static float QuintIn(float t) => t * t * t * t * t;

    private static float QuintOut(float t) => 1 - (float) Math.Pow(1 - t, 5);

    private static float QuintInOut(float t) => t < 0.5f ? 16 * t * t * t * t * t : 1 - (float) Math.Pow(-2 * t + 2, 5) / 2;

    private static float SineIn(float t) => 1 - (float) Math.Cos(t * Math.PI / 2);

    private static float SineOut(float t) => (float) Math.Sin(t * Math.PI / 2);

    private static float SineInOut(float t) => -(float) (Math.Cos(Math.PI * t) - 1) / 2;

    private static float ExpoIn(float t) => t == 0 ? 0 : (float) Math.Pow(2, 10 * t - 10);

    private static float ExpoOut(float t) => t == 1 ? 1 : 1 - (float) Math.Pow(2, -10 * t);

    private static float ExpoInOut(float t) => t == 0 
        ? 0 
        : t == 1 
        ? 1 
        : t < 0.5 
        ? (float) Math.Pow(2, 20 * t - 10) / 2 
        : (2 - (float) Math.Pow(2, -20 * t + 10)) / 2;

    private static float CircIn(float t) => 1 - (float) Math.Sqrt(1 - (t * t));

    private static float CircOut(float t) => (float) Math.Sqrt(1 - (t - 1) * (t - 1));

    private static float CircInOut(float t) => t < 0.5
        ? (1 - (float) Math.Sqrt(1 - (2 * t) * (2 * t))) / 2
        : ((float) Math.Sqrt(1 - (2 * t - 2) * (2 * t - 2)) + 1) / 2;

    private static float BackIn(float t) => 2.70158f * t * t * t - 1.70158f * t * t;

    private static float BackOut(float t) => 1 + 2.70158f * (float) Math.Pow(t - 1, 3) + 1.70158f * (float) Math.Pow(t - 1, 2);

    private static float BackInOut(float t) => t < 0.5
        ? (float) (Math.Pow(2 * t, 2) * (7.189819f * t - 2.5949095f)) / 2
        : (float) (Math.Pow(2 * t - 2, 2) * (7.189819f * (t - 1) + 2.5949095f) + 2) / 2;

    private static float BounceOut(float t)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (t < 1 / d1)
            return n1 * t * t;
        else if (t < 2 / d1)
            return n1 * (t -= 1.5f / d1) * t + 0.75f;
        else if (t < 2.5 / d1)
            return n1 * (t -= 2.25f / d1) * t + 0.9375f;
        else
            return n1 * (t -= 2.625f / d1) * t + 0.984375f;
    }

    private static float BounceIn(float t) => 1 - BounceOut(1 - t);
        
    private static float BounceInOut(float t) => t < 0.5
        ? (1 - BounceOut(1 - 2 * t)) / 2
        : (1 + BounceOut(2 * t - 1)) / 2;

    private static float ElasticIn(float t)
    {
        const float c4 = 2 * (float) Math.PI / 3;

        return t == 0
            ? 0
            : t == 1
            ? 1
            : -(float) (Math.Pow(2, 10 * t - 10) * Math.Sin((t * 10 - 10.75) * c4));
    }

    private static float ElasticOut(float t)
    {
        const float c4 = 2 * (float) Math.PI / 3;

        return t == 0
            ? 0
            : t == 1
            ? 1
            : (float) (Math.Pow(2, -10 * t) * Math.Sin((t * 10 - 0.75) * c4) + 1);
    }

    private static float ElasticInOut(float t)
    {
        const float c5 = 2 * (float) Math.PI / 4.5f;

        return t == 0
            ? 0
            : t == 1
            ? 1
            : t < 0.5
            ? -(float) (Math.Pow(2, 20 * t - 10) * Math.Sin((20 * t - 11.125) * c5)) / 2
            : (float) (Math.Pow(2, -20 * t + 10) * Math.Sin((20 * t - 11.125) * c5)) / 2 + 1;
    }

}

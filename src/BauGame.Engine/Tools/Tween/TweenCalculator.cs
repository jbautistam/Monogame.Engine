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
    ///     Calcula el valor interpolado para un float
    /// </summary>
    public static TweenResult<float> CalculateFloat(float elapsedTime, float duration, float from, float to, Func<float, float> easeFunction)
    {
        if (duration <= 0)
            return new TweenResult<float>(to, true, 1.0f);
        else
        {
            float progress = MathHelper.Clamp(elapsedTime / duration, 0f, 1f);
            bool isComplete = progress >= 1.0f;
            
            if (isComplete)
            {
                return new TweenResult<float>(to, true, 1.0f);
            }

            float easedProgress = easeFunction(progress);
            float value = MathHelper.Lerp(from, to, easedProgress);

            return new TweenResult<float>(value, isComplete, progress);
        }
    }

    /// <summary>
    /// Calcula el valor interpolado para un Vector2
    /// </summary>
    public static TweenResult<Vector2> CalculateVector2(float elapsedTime, float duration, Vector2 from, Vector2 to, Func<float, float> easeFunction)
    {
        if (duration <= 0)
        {
            return new TweenResult<Vector2>(to, true, 1.0f);
        }

        float progress = MathHelper.Clamp(elapsedTime / duration, 0f, 1f);
        bool isComplete = progress >= 1.0f;
            
        if (isComplete)
        {
            return new TweenResult<Vector2>(to, true, 1.0f);
        }

        float easedProgress = easeFunction(progress);
        Vector2 value = Vector2.Lerp(from, to, easedProgress);

        return new TweenResult<Vector2>(value, isComplete, progress);
    }

    /// <summary>
    ///     Calcula el valor interpolado para un Vector3
    /// </summary>
    public static TweenResult<Vector3> CalculateVector3(float elapsedTime, float duration, Vector3 from, Vector3 to, Func<float, float> easeFunction)
    {
        if (duration <= 0)
        {
            return new TweenResult<Vector3>(to, true, 1.0f);
        }

        float progress = MathHelper.Clamp(elapsedTime / duration, 0f, 1f);
        bool isComplete = progress >= 1.0f;
            
        if (isComplete)
        {
            return new TweenResult<Vector3>(to, true, 1.0f);
        }

        float easedProgress = easeFunction(progress);
        Vector3 value = Vector3.Lerp(from, to, easedProgress);

        return new TweenResult<Vector3>(value, isComplete, progress);
    }

    /// <summary>
    /// Calcula el valor interpolado para un Color
    /// </summary>
    public static TweenResult<Color> CalculateColor(float elapsedTime, float duration, Color from, Color to, Func<float, float> easeFunction)
    {
        if (duration <= 0)
        {
            return new TweenResult<Color>(to, true, 1.0f);
        }

        float progress = MathHelper.Clamp(elapsedTime / duration, 0f, 1f);
        bool isComplete = progress >= 1.0f;
            
        if (isComplete)
        {
            return new TweenResult<Color>(to, true, 1.0f);
        }

        float easedProgress = easeFunction(progress);
        Color value = Color.Lerp(from, to, easedProgress);

        return new TweenResult<Color>(value, isComplete, progress);
    }

    /// <summary>
    /// Calcula el valor interpolado para un Rectangle
    /// </summary>
    public static TweenResult<Rectangle> CalculateRectangle(float elapsedTime, float duration, Rectangle from, Rectangle to, Func<float, float> easeFunction)
    {
        if (duration <= 0)
        {
            return new TweenResult<Rectangle>(to, true, 1.0f);
        }

        float progress = MathHelper.Clamp(elapsedTime / duration, 0f, 1f);
        bool isComplete = progress >= 1.0f;
            
        if (isComplete)
        {
            return new TweenResult<Rectangle>(to, true, 1.0f);
        }

        float easedProgress = easeFunction(progress);
            
        int x = (int)MathHelper.Lerp(from.X, to.X, easedProgress);
        int y = (int)MathHelper.Lerp(from.Y, to.Y, easedProgress);
        int width = (int)MathHelper.Lerp(from.Width, to.Width, easedProgress);
        int height = (int)MathHelper.Lerp(from.Height, to.Height, easedProgress);
            
        Rectangle value = new Rectangle(x, y, width, height);

        return new TweenResult<Rectangle>(value, isComplete, progress);
    }

    /// <summary>
    /// Calcula un tween de float con tipo de easing predefinido
    /// </summary>
    public static TweenResult<float> CalculateFloat(float elapsedTime, float duration, float from, float to, EaseType easeType = EaseType.Linear)
    {
        return CalculateFloat(elapsedTime, duration, from, to, GetEaseFunction(easeType));
    }

    /// <summary>
    /// Calcula un tween de Vector2 con tipo de easing predefinido
    /// </summary>
    public static TweenResult<Vector2> CalculateVector2(float elapsedTime, float duration, Vector2 from, Vector2 to, EaseType easeType = EaseType.Linear)
    {
        return CalculateVector2(elapsedTime, duration, from, to, GetEaseFunction(easeType));
    }

    /// <summary>
    /// Obtiene la función de easing correspondiente al tipo
    /// </summary>
    private static Func<float, float> GetEaseFunction(EaseType easeType)
    {
        return easeType switch
        {
            EaseType.Linear => EaseFunctions.Linear,
            EaseType.QuadIn => EaseFunctions.QuadIn,
            EaseType.QuadOut => EaseFunctions.QuadOut,
            EaseType.QuadInOut => EaseFunctions.QuadInOut,
            EaseType.CubicIn => EaseFunctions.CubicIn,
            EaseType.CubicOut => EaseFunctions.CubicOut,
            EaseType.CubicInOut => EaseFunctions.CubicInOut,
            EaseType.QuartIn => EaseFunctions.QuartIn,
            EaseType.QuartOut => EaseFunctions.QuartOut,
            EaseType.QuartInOut => EaseFunctions.QuartInOut,
            EaseType.QuintIn => EaseFunctions.QuintIn,
            EaseType.QuintOut => EaseFunctions.QuintOut,
            EaseType.QuintInOut => EaseFunctions.QuintInOut,
            EaseType.SineIn => EaseFunctions.SineIn,
            EaseType.SineOut => EaseFunctions.SineOut,
            EaseType.SineInOut => EaseFunctions.SineInOut,
            EaseType.ExpoIn => EaseFunctions.ExpoIn,
            EaseType.ExpoOut => EaseFunctions.ExpoOut,
            EaseType.ExpoInOut => EaseFunctions.ExpoInOut,
            EaseType.CircIn => EaseFunctions.CircIn,
            EaseType.CircOut => EaseFunctions.CircOut,
            EaseType.CircInOut => EaseFunctions.CircInOut,
            EaseType.BackIn => EaseFunctions.BackIn,
            EaseType.BackOut => EaseFunctions.BackOut,
            EaseType.BackInOut => EaseFunctions.BackInOut,
            EaseType.ElasticIn => EaseFunctions.ElasticIn,
            EaseType.ElasticOut => EaseFunctions.ElasticOut,
            EaseType.ElasticInOut => EaseFunctions.ElasticInOut,
            EaseType.BounceIn => EaseFunctions.BounceIn,
            EaseType.BounceOut => EaseFunctions.BounceOut,
            EaseType.BounceInOut => EaseFunctions.BounceInOut,
            _ => EaseFunctions.Linear
        };
    }
}

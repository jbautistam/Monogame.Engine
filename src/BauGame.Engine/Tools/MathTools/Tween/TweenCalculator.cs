using Microsoft.Xna.Framework;
using Bau.BauEngine.Tools.MathTools.Easing;

namespace Bau.BauEngine.Tools.MathTools.Tween;

/// <summary>
///     Funciones de cálculo de valores tween
/// </summary>
public static class TweenCalculator
{
    /// <summary>
    ///     Calcula un valor interpolado de float con una función predefinida
    /// </summary>
    public static TweenResult<float> CalculateFloat(float elapsedTime, float duration, float from, float to, 
                                                    EasingFunctionsHelper.EasingType easeType = EasingFunctionsHelper.EasingType.Linear)
    {
        return CalculateFloat(elapsedTime, duration, from, to, EasingFunctionsHelper.GetEasingFunction(easeType));
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
    public static TweenResult<Vector2> CalculateVector2(float elapsedTime, float duration, Vector2 from, Vector2 to, 
                                                        EasingFunctionsHelper.EasingType easeType = EasingFunctionsHelper.EasingType.Linear)
    {
        return CalculateVector2(elapsedTime, duration, from, to, EasingFunctionsHelper.GetEasingFunction(easeType));
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
    ///     Calcula el valor interpolado para un Color
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
                                                                      (int) MathHelper.Lerp(from.Height, to.Height, amount)), 
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
    private static TweenResult<TypeData> EndResult<TypeData>(TypeData to) => new(to, 1.0f);

    /// <summary>
    ///     Obtiene un resultado intermedido
    /// </summary>
    private static TweenResult<TypeData> ProgressResult<TypeData>(TypeData to, float progress) => new(to, progress);
}

namespace UI.CharactersEngine.MathTools.Easing;

/// <summary>
///     Funciones de Tween
/// </summary>
public static class TweenFunctions
{
    /// <summary>
    ///     Ping Pong - va y vuelve
    /// </summary>
    public static float PingPong(float t) => t < 0.5f ? t * 2 : 2 - t * 2;
    
    /// <summary>
    ///     Bucle (para animaciones cíclicas)
    /// </summary>
    public static float Loop(float t, int cycles = 1) => (t * cycles) % 1;
    
    /// <summary>
    ///     Ping Pong con función de easing
    /// </summary>
    public static Func<float, float> Yoyo(Func<float, float> ease) => t => t < 0.5f ? ease(t * 2) : ease(2 - t * 2);
    
    /// <summary>
    ///     Espera antes de comenzar el tween
    /// </summary>
    public static Func<float, float> Delay(Func<float, float> ease, float delay) => t => t < delay ? 0 : ease((t - delay) / (1 - delay));
    
    /// <summary>
    ///     Oscila alrededor del valor final
    /// </summary>
    public static Func<float, float> Wobble(Func<float, float> ease, float frequency, float amplitude) => t =>
    {
        float baseValue = ease(t);
        float wobble = MathF.Sin(t * frequency * MathF.PI * 2) * amplitude * (1 - t);

            // Devuelve el valor final
            return baseValue + wobble;
    };
    
    /// <summary>
    ///     Encadena dos funciones de easing
    /// </summary>
    public static Func<float, float> Chain(Func<float, float> first, Func<float, float> second, float split = 0.5f) => t =>
    {
        if (t < split)
            return first(t / split) * 0.5f;
        else
            return 0.5f + second((t - split) / (1 - split)) * 0.5f;
    };
}
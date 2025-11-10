namespace Bau.Libraries.BauGame.Engine.Tools.Tween;

/// <summary>
///     Resultado de un cálculo de una función tween
/// </summary>
public struct TweenResult<T>(T value, float progress)
{
    /// <summary>
    ///     Valor interpolado en el tiempo actual
    /// </summary>
    public T Value { get; set; } = value;
    
    /// <summary>
    ///     Progreso normalizado (0.0 a 1.0)
    /// </summary>
    public float Progress { get; set; } = progress;
    
    /// <summary>
    ///     Indica si el proceso de tween ha terminado
    /// </summary>
    public bool IsComplete => Progress >= 1;
}

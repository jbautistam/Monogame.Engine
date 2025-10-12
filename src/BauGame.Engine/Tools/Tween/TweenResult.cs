namespace Bau.Libraries.BauGame.Engine.Tools.Tween;

/// <summary>
///     Resultado de un cálculo de una función tween
/// </summary>
public struct TweenResult<T>(T value, bool isComplete, float progress)
{
    /// <summary>
    ///     Valor interpolado en el tiempo actual
    /// </summary>
    public T Value { get; set; } = value;
    
    /// <summary>
    ///     Indica si el tween ha terminado
    /// </summary>
    public bool IsComplete { get; set; } = isComplete;
    
    /// <summary>
    ///     Progreso normalizado (0.0 a 1.0)
    /// </summary>
    public float Progress { get; set; } = progress;
}

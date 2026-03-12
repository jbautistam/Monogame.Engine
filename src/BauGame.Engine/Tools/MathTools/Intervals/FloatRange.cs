namespace Bau.Libraries.BauGame.Engine.Tools.MathTools.Intervals;

/// <summary>
///     Rango de valores decimales
/// </summary>
public readonly record struct FloatRange(float Min, float Max)
{
    /// <summary>
    ///     Obtiene un valor aleatorio
    /// </summary>
    public float GetValue()
    {
        if (Min == Max)
            return Min;
        else
            return Min + (float) (Randomizer.Random.NextDouble() * (Max - Min));
    }
}
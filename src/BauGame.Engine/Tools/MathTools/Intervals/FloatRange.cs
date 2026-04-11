namespace Bau.BauEngine.Tools.MathTools.Intervals;

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
            return Randomizer.GetRandom(Min, Max);
    }
}
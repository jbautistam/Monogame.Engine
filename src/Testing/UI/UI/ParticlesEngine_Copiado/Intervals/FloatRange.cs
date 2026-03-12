namespace UI.CharactersEngine.ParticlesEngine.Intervals;

/// <summary>
///     Rango de valores decimales
/// </summary>
public readonly record struct FloatRange(float Min, float Max)
{
    /// <summary>
    ///     Obtiene un valor aleatorio
    /// </summary>
    public float GetValue(Random random)
    {
        if (Min == Max)
            return Min;
        else
            return Min + (float) (random.NextDouble() * (Max - Min));
    }
}
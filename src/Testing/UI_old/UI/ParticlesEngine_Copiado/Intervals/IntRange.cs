namespace UI.CharactersEngine.ParticlesEngine.Intervals;

/// <summary>
///     Rango de valores enteros
/// </summary>
public readonly record struct IntRange(int Min, int Max)
{
    /// <summary>
    ///     Obtiene un valor aleatorio en el rango
    /// </summary>
    public int GetValue(Random random)
    {
        if (Min == Max)
            return Min;
        else
            return random.Next(Min, Max + 1);
    }
}

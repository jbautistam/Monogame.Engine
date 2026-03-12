namespace Bau.Libraries.BauGame.Engine.Tools.MathTools.Intervals;

/// <summary>
///     Rango de valores enteros
/// </summary>
public readonly record struct IntRange(int Min, int Max)
{
    /// <summary>
    ///     Obtiene un valor aleatorio en el rango
    /// </summary>
    public int GetValue()
    {
        if (Min == Max)
            return Min;
        else
            return Randomizer.Random.Next(Min, Max + 1);
    }
}

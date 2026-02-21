using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.Common;

/// <summary>
///		Registro de rango
/// </summary>
public struct RangeStruct<TypeData>(TypeData minimum, TypeData maximum)
{
    /// <summary>
    ///     Obtiene un valor aleatorio de un rango
    /// </summary>
    public float GetValue()
    {
        if (Minimum is float minimum && Maximum is float maximum)
        {
            if (minimum == maximum)
                return minimum;
            else
                return Tools.Randomizer.GetRandom(minimum, maximum);
        }
        else
            return 0;
    }

    /// <summary>
    ///     Obtiene un color teniendo en cuenta un rango
    /// </summary>
	public Color GetColor()
    {
        if (Minimum is Color minimum && Maximum is Color maximum)
            return Tools.Randomizer.GetRandomColor(minimum, maximum);
        else
            return Color.White;
    }

    /// <summary>
    ///     Obtiene un vector teniendo en cuenta un rango
    /// </summary>
	public Vector2 GetVector() => new(GetValue(), GetValue());

	/// <summary>
	///		Mínimo
	/// </summary>
	public TypeData Minimum { get; } = minimum;

	/// <summary>
	///		Máximo
	/// </summary>
	public TypeData Maximum { get; } = maximum;
}

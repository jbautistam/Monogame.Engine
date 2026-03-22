namespace Bau.BauEngine.Tools.Extensors;

/// <summary>
///		Extensores de <see cref="string"/>
/// </summary>
public static class StringExtensors
{
	/// <summary>
	///		Comprueba si dos cadenas son iguales sin tener en cuenta los nulos
	/// </summary>
	public static bool EqualsIgnoreNull(this string? first, string? second, StringComparison comparison)
	{
		if (string.IsNullOrEmpty(first) && !string.IsNullOrEmpty(second))
			return false;
		else if (!string.IsNullOrEmpty(first) && string.IsNullOrEmpty(second))
			return false;
		else if (string.IsNullOrEmpty(first) && string.IsNullOrEmpty(second))
			return true;
		else
			return first!.Equals(second, comparison);
	}

	/// <summary>
	///		Obtiene un valor enumerado a partir de una cadena
	/// </summary>
	// TODO: esto debería ir a un extensor de cadenas
	public static TypeEnum GetEnum<TypeEnum>(this string value, TypeEnum defaultValue) where TypeEnum : struct
	{
		if (Enum.TryParse<TypeEnum>(value, ignoreCase: true, out var result))
			return result;
		else
			return defaultValue;
	}
}

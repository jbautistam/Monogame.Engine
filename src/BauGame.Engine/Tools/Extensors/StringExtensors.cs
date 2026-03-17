namespace Bau.Libraries.BauGame.Engine.Tools.Extensors;

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
}

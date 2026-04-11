using Microsoft.Xna.Framework;
using Bau.Libraries.LibHelper.Extensors;
using Bau.BauEngine.Entities.UserInterface;
using Bau.BauEngine.Entities.Common;
using Bau.BauEngine.Tools.MathTools.Intervals;

namespace Bau.BauEngine.Repositories.Xml;

/// <summary>
///		Helper para los repositorios
/// </summary>
public class RepositoryXmlHelper
{
	/// <summary>
	///		Carga un color
	/// </summary>
	public Color GetColor(string value, Color defaultColor) => Bau.BauEngine.Tools.Conversors.ColorConversor.Parse(value, defaultColor);

	/// <summary>
	///		Carga un margen de una cadena
	/// </summary>
	public UiMargin GetMargin(string value)
	{
		// Interpreta una cadena con el margen
		if (!string.IsNullOrWhiteSpace(value))
		{
			string [] parts = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

				if (parts.Length == 1)
					return new UiMargin(parts[0].GetInt(0));
				else if (parts.Length == 2)
					return new UiMargin(parts[0].GetInt(0), parts[1].GetInt(0), parts[0].GetInt(0), parts[1].GetInt(0));
				else if (parts.Length == 4)
					return new UiMargin(parts[0].GetInt(0), parts[1].GetInt(0), parts[2].GetInt(0), parts[3].GetInt(0));
		}
		// Devuelve el margen predeterminado
		return new UiMargin();
	}

	/// <summary>
	///		Carga un rectángulo
	/// </summary>
	public Rectangle GetRectangle(string value)
	{
		// Interpreta una cadena con un rectángulo
		if (!string.IsNullOrWhiteSpace(value))
		{
			string [] parts = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

				if (parts.Length == 4)
					return new Rectangle(parts[0].GetInt(0), parts[1].GetInt(0), parts[2].GetInt(0), parts[3].GetInt(0));
		}
		// Devuelve el rectángulo predeterminado
		return new Rectangle();
	}

	/// <summary>
	///		Carga un rectángulo
	/// </summary>
	public RectangleF GetRectangleF(string value)
	{
		// Interpreta una cadena con un rectángulo
		if (!string.IsNullOrWhiteSpace(value))
		{
			string [] parts = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

				if (parts.Length == 4)
					return new RectangleF((float) parts[0].GetDouble(0), (float) parts[1].GetDouble(0), (float) parts[2].GetDouble(0), (float) parts[3].GetDouble(0));
		}
		// Devuelve el rectángulo predeterminado
		return new RectangleF(0, 0, 0, 0);
	}

	/// <summary>
	///		Obtiene una posición
	/// </summary>
	public UiPosition GetPosition(string value)
	{
		// Interpreta una cadena con una posición
		if (!string.IsNullOrWhiteSpace(value))
		{
			string [] parts = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

				if (parts.Length == 4)
					return new UiPosition((float) parts[0].GetDouble(0), (float) parts[1].GetDouble(0), 
										  (float) parts[2].GetDouble(0), (float) parts[3].GetDouble(0));
		}
		// Devuelve el rectángulo predeterminado
		return new UiPosition(0, 0, 1, 1);
	}

	/// <summary>
	///		Carga un vector
	/// </summary>
	public Vector2 GetVector(string value) => GetVector(value, Vector2.Zero);

	/// <summary>
	///		Carga un vector
	/// </summary>
	public Vector2 GetVector(string value, Vector2 defaultValue)
	{
		// Interpreta una cadena con un vector
		if (!string.IsNullOrWhiteSpace(value))
		{
			string [] parts = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

				if (parts.Length == 2)
					return new Vector2((float) parts[0].GetDouble(0), (float) parts[1].GetDouble(0));
		}
		// Devuelve el vector predeterminado
		return defaultValue;
	}

	/// <summary>
	///		Obtiene una lista de valores
	/// </summary>
	public List<float> GetValues(string value)
	{
		List<float> values = [];

			// Carga la lista
			foreach (string part in value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
				values.Add((float) part.GetDouble(0));
			// Devuelve la lista de valores
			return values;
	}

	/// <summary>
	///		Obtiene un intervalo de valores decimales
	/// </summary>
	public FloatRange GetFloatInterval(string value, float defaultValue)
	{
		// Interpreta una cadena con un rango
		if (!string.IsNullOrWhiteSpace(value))
		{
			string [] parts = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

				if (parts.Length == 2)
					return new FloatRange((float) parts[0].GetDouble(0), (float) parts[1].GetDouble(0));
				else if (parts.Length == 1)
					return new FloatRange((float) parts[0].GetDouble(0), (float) parts[0].GetDouble(0));
		}
		// Devuelve el rango predeterminado
		return new FloatRange(defaultValue, defaultValue);
	}

	/// <summary>
	///		Obtiene un intervalo de valores enteros
	/// </summary>
	public IntRange GetIntInterval(string value, int defaultValue)
	{
		// Interpreta una cadena con un rango
		if (!string.IsNullOrWhiteSpace(value))
		{
			string [] parts = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

				if (parts.Length == 2)
					return new IntRange(parts[0].GetInt(0), parts[1].GetInt(0));
				else if (parts.Length == 1)
					return new IntRange(parts[0].GetInt(0), parts[0].GetInt(0));
		}
		// Devuelve el rango predeterminado
		return new IntRange(defaultValue, defaultValue);
	}

	/// <summary>
	///		Obtiene un intervalo de colores
	/// </summary>
	public ColorRange GetColorInterval(string value, Color defaultValue)
	{
		// Interpreta una cadena con un rango
		if (!string.IsNullOrWhiteSpace(value))
		{
			string [] parts = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

				if (parts.Length == 2)
					return new ColorRange(GetColor(parts[0], defaultValue), GetColor(parts[1], defaultValue));
				else if (parts.Length == 1)
					return new ColorRange(GetColor(parts[0], defaultValue), GetColor(parts[0], defaultValue));
		}
		// Devuelve el rango predeterminado
		return new ColorRange(defaultValue, defaultValue);
	}
}

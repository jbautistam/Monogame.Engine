using Microsoft.Xna.Framework;
using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface;

namespace EngineSample.Core.Configuration.Repositories;

/// <summary>
///		Helper para los repositorios
/// </summary>
internal class RepositoryHelper
{
	/// <summary>
	///		Carga un color
	/// </summary>
	internal Color GetColor(string value, Color defaultColor) => Bau.Libraries.BauGame.Engine.Tools.Conversors.ColorConversor.Parse(value, defaultColor);

	/// <summary>
	///		Carga un margen de una cadena
	/// </summary>
	internal UiMargin GetMargin(string value)
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
	internal Rectangle GetRectangle(string value)
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
	internal Vector2 GetVector(string value)
	{
		// Interpreta una cadena con un vector
		if (!string.IsNullOrWhiteSpace(value))
		{
			string [] parts = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

				if (parts.Length == 2)
					return new Vector2((float) parts[0].GetDouble(0), (float) parts[1].GetDouble(0));
		}
		// Devuelve el vector predeterminado
		return Vector2.Zero;
	}

	/// <summary>
	///		Obtiene una lista de valores
	/// </summary>
	internal List<float> GetValues(string value)
	{
		List<float> values = [];

			// Carga la lista
			foreach (string part in value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
				values.Add((float) part.GetDouble(0));
			// Devuelve la lista de valores
			return values;
	}
}

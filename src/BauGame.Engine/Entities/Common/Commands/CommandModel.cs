namespace Bau.BauEngine.Entities.Common.Commands;

/// <summary>
///		Lista de comandos
/// </summary>
public class CommandModel
{
	/// <summary>
	///		Interpreta el contenido
	/// </summary>
	public void Parse(string text)
	{
		if (!string.IsNullOrWhiteSpace(text))
		{
			int indexParenthesis = text.IndexOf('(');

				// Separa el nombre de los parámetros
				if (indexParenthesis < 0)
					Name = text;
				else
				{
					// Saca el nombre
					Name = text.Substring(0, indexParenthesis);
					// Recoge los parámetros
					if (text.Length > indexParenthesis)
						ExtractParameters(text.Substring(indexParenthesis + 1));
				}
		}
	}

	/// <summary>
	///		Extrae los parámetros
	/// </summary>
	private void ExtractParameters(string data)
	{
		// Quita el paréntesis final
		if (data.EndsWith(')'))
		{
			if (data.Length == 1)
				data = string.Empty;
			else
				data = data.Substring(0, data.Length - 1);
		}
		// Divide por las comas
		if (!string.IsNullOrWhiteSpace(data))
			foreach (string section in data.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
			{
				string [] parts = section.Split('=', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
				string key, value;

					// Divide en clave / valor
					if (parts.Length == 2)
					{
						key = parts[0];
						value = parts[1];
					}
					else
					{
						key = parts[0];
						value = string.Empty;
					}
					// Añade al diccionario
					if (Parameters.ContainsKey(key))
						Parameters[key] = value;
					else
						Parameters.Add(key, value);
			}
	}

	/// <summary>
	///		Comprueba si el comando tiene un nombre específico
	/// </summary>
	public bool IsOfType(string name) => !string.IsNullOrWhiteSpace(Name) && Name.Equals(name, StringComparison.CurrentCultureIgnoreCase);

	/// <summary>
	///		Obtiene la cadena de parámetro asociada a un comando
	/// </summary>
	public string? GetParameter(string name)
	{
		if (Parameters.TryGetValue(name, out string? value))
			return value;
		else
			return null;
	}

	/// <summary>
	///		Nombre del comando
	/// </summary>
	public string Name { get; private set; } = default!;

	/// <summary>
	///		Parámetros del comando
	/// </summary>
	public Dictionary<string, string?> Parameters { get; } = new(StringComparer.CurrentCultureIgnoreCase);
}

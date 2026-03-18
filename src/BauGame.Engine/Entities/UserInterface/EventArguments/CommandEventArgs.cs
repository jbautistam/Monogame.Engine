namespace Bau.BauEngine.Entities.UserInterface.EventArguments;

/// <summary>
///		Argumentos del evento de ejecución de un comando
/// </summary>
public class CommandEventArgs(UiElement component, string data) : EventArgs
{
	/// <summary>
	///		Interpreta el contenido
	/// </summary>
	public (string name, Dictionary<string, string>) Parse()
	{
		string name = string.Empty;
		Dictionary<string, string> parameters = [];

			// Interpreta los datos
			if (!string.IsNullOrWhiteSpace(Data))
			{
				int indexParenthesis = Data.IndexOf('(');

					// Separa el nombre de los parámetros
					if (indexParenthesis < 0)
						name = Data;
					else
					{
						// Saca el nombre
						name = Data.Substring(0, indexParenthesis);
						// Recoge los parámetros
						if (Data.Length > indexParenthesis)
							parameters = ExtractParameters(Data.Substring(indexParenthesis + 1));
					}
			}
			// Devuelve los datos interpretados
			return (name, parameters);
	}

	/// <summary>
	///		Extrae los parámetros
	/// </summary>
	private Dictionary<string, string> ExtractParameters(string data)
	{
		Dictionary<string, string> parameters = new(StringComparer.CurrentCultureIgnoreCase);

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
						if (parameters.ContainsKey(key))
							parameters[key] = value;
						else
							parameters.Add(key, value);
				}
			// Devuelve los parámetros
			return parameters;
	}

	/// <summary>
	///		Componente que lanza el comando
	/// </summary>
	public UiElement Component { get; } = component;

	/// <summary>
	///		Datos del comando
	/// </summary>
	public string Data { get; } = data;
}
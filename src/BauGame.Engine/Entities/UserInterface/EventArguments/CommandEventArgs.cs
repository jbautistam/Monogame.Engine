namespace Bau.BauEngine.Entities.UserInterface.EventArguments;

/// <summary>
///		Argumentos del evento de ejecución de un comando
/// </summary>
public class CommandEventArgs(UiElement component, string data) : EventArgs
{
	/// <summary>
	///		Interpreta los datos en una lista de comandos
	/// </summary>
	public Common.Commands.CommandModel Parse()
	{
		Common.Commands.CommandModel commands = new();

			// Interpreta el texto
			commands.Parse(Data);
			// Devuelve los comandos
			return commands;
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
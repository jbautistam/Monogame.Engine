namespace UI.Components.TypeWriter;

/// <summary>
///		Comando de la máquina para esperar un tiempo
/// </summary>
internal class CommandWaitTypeWriter : CommandAbstractLine
{
	/// <summary>
	///		Tiempo que se va a esperar
	/// </summary>
	internal required float Time { get; init; }
}
namespace UI.Components.TypeWriter;

/// <summary>
///		Comando para cambiar la velocidad de la máquina de escribir
/// </summary>
internal class CommandSpeedTypeWriter : CommandAbstractLine
{
	/// <summary>
	///		Multiplicador de la velocidad
	/// </summary>
	internal required float Speed { get; init; }
}
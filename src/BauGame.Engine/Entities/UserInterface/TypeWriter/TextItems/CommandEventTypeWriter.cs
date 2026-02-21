namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.TypeWriter.TextItems;

/// <summary>
///		Comando de la máquina para lanzar un comando
/// </summary>
internal class CommandEvetTypeWriter : CommandAbstractLine
{
	/// <summary>
	///		Datos del evento
	/// </summary>
	internal required string Data { get; init; }
}
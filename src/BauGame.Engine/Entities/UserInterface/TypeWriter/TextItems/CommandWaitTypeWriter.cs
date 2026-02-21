namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.TypeWriter.TextItems;

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
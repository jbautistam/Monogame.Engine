namespace Bau.Libraries.BauGame.Engine.Scenes.Messages;

/// <summary>
///		Mensajes enviados entre actores o capas
/// </summary>
public class MessageModel(Actors.AbstractActor sender, string type)
{
	/// <summary>
	///		Actor que envía el mensaje
	/// </summary>
	public Actors.AbstractActor Sender { get; } = sender;

	/// <summary>
	///		Tipo del mensaje
	/// </summary>
	public string Type { get; } = type;

	/// <summary>
	///		Contenido del mensaje
	/// </summary>
	public required string Message { get; init; }

	/// <summary>
	///		Datos adicionales
	/// </summary>
	public object? Tag { get; set; }
}

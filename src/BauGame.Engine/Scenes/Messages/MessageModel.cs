using Bau.Libraries.BauGame.Engine.Managers;

namespace Bau.Libraries.BauGame.Engine.Scenes.Messages;

/// <summary>
///		Mensajes enviados entre actores o capas
/// </summary>
public class MessageModel(Actors.AbstractActor sender, string type, string to) : Entities.Common.Collections.ISecureListItem
{
	/// <summary>
	///		Arranca el objeto (necesario para la interface)
	/// </summary>
	public void Start()
	{
	}

	/// <summary>
	///		Detiene el objeto (necesario para la interface)
	/// </summary>
	public void End(GameContext gameContext)
	{
	}

	/// <summary>
	///		Id del mensaje
	/// </summary>
	public string Id { get; } = Guid.NewGuid().ToString();

	/// <summary>
	///		Actor que envía el mensaje
	/// </summary>
	public Actors.AbstractActor Sender { get; } = sender;

	/// <summary>
	///		Destinatario del mensaje
	/// </summary>
	public string To { get; } = to;

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

	/// <summary>
	///		Momento en que se ha creado el mensaje
	/// </summary>
	public float CreatedAt { get; set; }
}

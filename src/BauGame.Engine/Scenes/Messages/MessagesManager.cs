using Bau.Libraries.BauGame.Engine.Managers;

namespace Bau.Libraries.BauGame.Engine.Scenes.Messages;

/// <summary>
///		Manager de mensajes
/// </summary>
public class MessagesManager(AbstractScene scene) : Entities.Common.Collections.SecureList<MessageModel>
{
    // Registros
    private record Received(string To, string? Type, Guid? Guid);
    // Variables privadas
    private float _elapsedTime;

    /// <summary>
    ///     Actualiza el manager
    /// </summary>
	protected override void UpdateSelf(GameContext gameContext)
	{
        // Marca para eliminar todos los mensajes que hayan pasado el tiempo
        foreach (MessageModel message in Enumerate())
            if (message.CreatedAt + _elapsedTime > 2)
                MarkToDestroy(message, TimeSpan.FromMilliseconds(1));
        // Incrementa el tiempo pasado
        _elapsedTime += gameContext.DeltaTime;
	}

    /// <summary>
    ///     Trata el elemento añadido
    /// </summary>
	protected override void Added(MessageModel item)
	{
        item.CreatedAt = _elapsedTime;
	}

    /// <summary>
    ///     Trata el elemento eliminado
    /// </summary>
	protected override void Removed(MessageModel item)
	{
	}

    /// <summary>
    ///     Envía un mensaje a un destinatario
    /// </summary>
    public void SendMessage(Actors.AbstractActor sender, string to, string type, string message, object? tag = null)
    {
        Add(new MessageModel(sender, type, to)
                    {
                        Message = message,
                        Tag = tag
                    }
            );
    }

	/// <summary>
	///     Obtiene la lista de mensajes recibidos de un tipo ordenados ascendentemente por el momento de la creación
	/// </summary>
	public List<MessageModel> GetReceived(string to, string? type = null)
	{
		List<MessageModel> messages = [];

            // Obtiene los mensajes
            foreach (MessageModel message in Enumerate())
                if (message.To.Equals(to, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (string.IsNullOrWhiteSpace(type) || message.Type.Equals(type, StringComparison.CurrentCultureIgnoreCase))
                        messages.Add(message);
                }
            // Ordena los mensajes
            messages.Sort((first, second) => first.CreatedAt.CompareTo(second.CreatedAt));
            // Marca los mensajes como recibidos
            MarkReceived(messages);
            // Devuelve los mensajes
            return messages;
	}

    /// <summary>
    ///     Obtiene el último mensaje recibido de un tipo (borra el resto de mensajes)
    /// </summary>
	public MessageModel? GetLastReceived(string to, string? type = null)
	{
		List<MessageModel> messages = GetReceived(to, type);

            // Devuelve el último mensaje de la lista
            if (messages.Count > 0)
                return messages[^1];
            else
                return null;
	}

    /// <summary>
    ///     Marca como recibidos una serie de mensajes
    /// </summary>
	private void MarkReceived(List<MessageModel> messages)
	{
		foreach (MessageModel message in messages)
            MarkToDestroy(message, TimeSpan.FromMilliseconds(0));
	}

	/// <summary>
	///		Escena a la que se asocian los mensajes
	/// </summary>
	public AbstractScene Scene { get; } = scene;
}

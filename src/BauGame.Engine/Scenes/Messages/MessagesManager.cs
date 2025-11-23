using Bau.Libraries.BauGame.Engine.Managers;

namespace Bau.Libraries.BauGame.Engine.Scenes.Messages;

/// <summary>
///		Manager de mensajes
/// </summary>
public class MessagesManager(AbstractScene scene)
{
    // Registros
    private record Received(string To, string? Type, Guid? Guid);

    /// <summary>
    ///     Actualiza el manager
    /// </summary>
	public void Update(GameContext gameContext)
	{
        foreach (Received received in ReceivedMessages)
            if (Messages.TryGetValue(received.To, out List<MessageModel>? messages))
            {
                // Borra todos los mensajes o los que sean para ese destinatario y del tipo especificado o con el Guid indicado
                if (string.IsNullOrWhiteSpace(received.Type) && received.Guid is null)
                    Messages.Remove(received.To);
                else
                    for (int index = messages.Count - 1; index >= 0; index--)
                        if ((received.Guid is not null && messages[index].Id == received.Guid) ||
                                (!string.IsNullOrWhiteSpace(received.Type) && messages[index].Type.Equals(received.Type, StringComparison.CurrentCultureIgnoreCase)))
                            messages.RemoveAt(index);
            }
	}

    /// <summary>
    ///     Envía un mensaje a un destinatario
    /// </summary>
    public void SendMessage(string to, MessageModel message)
    {
        SendMessages(to, [ message ]);
    }

    /// <summary>
    ///     Envía una serie de mensajes a un destinatario
    /// </summary>
    public void SendMessages(string to, List<MessageModel> messages)
    {
        if (Messages.ContainsKey(to))
            Messages[to].AddRange(messages);
        else
            Messages.Add(to, messages);
    }

    /// <summary>
    ///     Obtiene la lista de mensajes recibidos de un tipo
    /// </summary>
	public List<MessageModel> GetReceived(string to, string? type = null)
	{
		List<MessageModel> messages = [];

            // Obtiene los mensajes
            if (Messages.TryGetValue(to, out List<MessageModel>? receivedMessages))
            {
                if (string.IsNullOrWhiteSpace(type))
                    messages.AddRange(receivedMessages);
                else
                    foreach (MessageModel receivedMessage in receivedMessages)
                        if (receivedMessage.Type.Equals(type, StringComparison.CurrentCultureIgnoreCase))
                            messages.Add(receivedMessage);
            }
            // Devuelve los mensajes
            return messages;
	}

    /// <summary>
    ///     Obtiene el primer mensaje recibido de un tipo
    /// </summary>
	public MessageModel? GetFirstReceived(string to, string type)
	{
        if (Messages.TryGetValue(to, out List<MessageModel>? receivedMessages))
            return receivedMessages.FirstOrDefault(item => item.Type.Equals(type, StringComparison.CurrentCultureIgnoreCase));
        else
            return null;
	}

    /// <summary>
    ///     Marca los mensajes a un destinatario como recibidos
    /// </summary>
	public void MarkReceived(string to, string? type = null, Guid? guid = null)
	{
		ReceivedMessages.Add(new Received(to, type, guid));
	}

    /// <summary>
    ///     Marca como recibidos una serie de mensajes
    /// </summary>
	public void MarkReceived(string to, List<MessageModel> messages)
	{
		foreach (MessageModel message in messages)
            MarkReceived(to, null, message.Id);
	}

	/// <summary>
	///		Escena a la que se asocian los mensajes
	/// </summary>
	public AbstractScene Scene { get; } = scene;

    /// <summary>
    ///     Mensajes recibidos en la capa
    /// </summary>
    private Dictionary<string, List<MessageModel>> Messages { get; } = new(StringComparer.CurrentCultureIgnoreCase);

    /// <summary>
    ///     Mensajes recibidos
    /// </summary>
    private List<Received> ReceivedMessages { get; } = [];
}

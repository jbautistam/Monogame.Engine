using Microsoft.Xna.Framework;
using Bau.BauEngine.Managers;
using Bau.BauEngine.Tools.MathTools.Tween;

namespace Bau.BauEngine.Entities.UserInterface.Popups.MobileChats;

/// <summary>
///		Componente para mostrar una conversación de móvil
/// </summary>
public class UiMobileChat(Scenes.Layers.AbstractUserInterfaceLayer layer, UiPosition position) : UiElement(layer, position)
{
    // Variables privadas
    private List<MobileSender> _senders = [];
    private List<MobileMessage> _messages = [];
    private float _typingElapsed = 8f;

    /// <summary>
    ///     Añade un participante
    /// </summary>
    public void AddParticipant(MobileSender sender)
    {
        // Elimina los participantes con el mismo nombre
        for (int index = _senders.Count - 1; index >= 0; index--)
            if (_senders[index].Name.Equals(sender.Name, StringComparison.CurrentCultureIgnoreCase))
                _senders.RemoveAt(index);
        // Añade el participante
        _senders.Add(sender);
    }

    /// <summary>
    ///     Añade un mensaje a la cola del chat
    /// </summary>
    public void AddMessage(string sender, string text, float timeToShow)
    {
        MobileSender? participant = _senders.FirstOrDefault(item => item.Name.Equals(sender, StringComparison.CurrentCultureIgnoreCase));

            // Crea el mensaje si viene de un participante conocido
            if (participant is not null)
            {
                MobileMessage message = new(participant, text, timeToShow);

                    // Calcula el tiempo que va a tardar en mostrarse
                    message.Start = Math.Max(0.5f, timeToShow) + ComputePreviousTime();
                    // Añade el mensaje a la lista
                    _messages.Add(message);
            }

        // Calcula el tiempo anterior a este mensaje
        float ComputePreviousTime()
        {
            float total = 0;

                // Acumula el tiempo a mostrar de todos los mensajes
                foreach (MobileMessage message in _messages)
                    if (message.TimeToShow > 0)
                        total += message.TimeToShow;
                // Devuelve el tiempo calculado
                return total;
        }
    }

    /// <summary>
    ///     Calcula los límites del control: invalida los mensajes para que se recalculen
    /// </summary>
    protected override void ComputeScreenBoundsSelf()
	{
        foreach (MobileMessage message in _messages)
            message.IsDirty = true;
	}

    /// <summary>
    ///     Obtiene el ancho máximo de un mensaje
    /// </summary>
    private int GetMaxBubbleWidth() => Position.ContentBounds.Width - AvatarSize - PaddingMessage.Right;

    /// <summary>
    ///     Actualiza los datos
    /// </summary>
	protected override void UpdateSelf(GameContext gameContext)
	{
        // Carga la fuente
        Font?.Update(gameContext);
        // Modifica el tiempo de presentación de los mensajes
        foreach (MobileMessage message in _messages)
            message.Start -= gameContext.DeltaTime;
        // Calcula la posición de los mensajes
        ComputeMessagesPosition();
        // Elimina los mensajes anteriores
        RemoveOldMessages();
    }

    /// <summary>
    ///     Calcula la posición de los mensajes
    /// </summary>
    private void ComputeMessagesPosition()
    {
        float y = Position.ContentBounds.Bottom - MessageSpacing;

            // Recorre los mensajes calculando la posición
            for (int index = _messages.Count - 1; index >= 0; index--)
                if (_messages[index].Status != MobileMessage.StatusType.Waiting)
                {
                    // Calcula la posición y
                    y -= _messages[index].GetHeight(Font, GetMaxBubbleWidth());
                    // Asigna la posición Y al mensaje
                    _messages[index].Y = y;
                    // Quita la separación entre mensajes
                    y -= MessageSpacing;
                }
    }

    /// <summary>
    ///     Elimina los mensajes que hayan salido de la pantalla
    /// </summary>
    private void RemoveOldMessages()
    {
        for (int index = _messages.Count - 1; index >= 0; index--)
            if (_messages[index].Status != MobileMessage.StatusType.Waiting && _messages[index].Y < Position.ContentBounds.Top)
                _messages.RemoveAt(index);
    }

    /// <summary>
    ///     Dibuja los mensajes
    /// </summary>
	public override void Draw(Scenes.Rendering.RenderingManager renderingManager, GameContext gameContext)
	{
        int width = GetMaxBubbleWidth();

            // Dibuja los datos dependiendo del estilo
            Layer.DrawStyle(renderingManager, Style, Styles.UiStyle.StyleType.Normal, Position.ContentBounds, gameContext);
            // Dibuja los mensajes
            if (Font is not null)
                foreach (MobileMessage message in _messages)
                    if (message.Status != MobileMessage.StatusType.Waiting)
                    {
                        Sprites.SpriteDefinition? avatar = GetAvatar(message);
                        Rectangle avatarBounds = ComputeAvatarBounds(avatar, message.Sender.IsPlayer, message.Y);
                        Rectangle bounds = new(GetMessageX(message, width), (int) message.Y, width, (int) message.GetHeight(Font, width));

                            // Dibuja el avatar
                            if (avatar is not null)
                                renderingManager.SpriteRenderer.Draw(avatar, avatarBounds, Vector2.Zero, 0, Color.White);
                            // Dibuja el contenido del mensaje
                            if (message.Status == MobileMessage.StatusType.Writing && !message.Sender.IsPlayer)
                                DrawWritting(renderingManager, bounds, gameContext);
                            else
                            {
                                // Dibuja el rectángulo de fondo
                                DrawBackground(renderingManager, message.Sender, bounds);
                                // Dibuja el texto del mensaje
                                DrawMessage(renderingManager, message, bounds);
                            }
                    }

        // Obtiene la coordenada X donde se debe mostrar un mensaje
        int GetMessageX(MobileMessage message, int messageWidth)
        {
            if (message.Sender.IsPlayer)
                return Position.ContentBounds.Right - AvatarSize - messageWidth - 3;
            else
                return AvatarSize + Position.ContentBounds.Left + 3;
        }

        // Calcula la posición del avatar
        Rectangle ComputeAvatarBounds(Sprites.SpriteDefinition? avatar, bool isPlayer, float y)
        {
            if (avatar is not null)
            {
                if (isPlayer)
                    return new Rectangle(Position.ContentBounds.Right - AvatarSize, (int) y, AvatarSize, AvatarSize);
                else
                    return new Rectangle(Position.ContentBounds.X + 1, (int) y, AvatarSize, AvatarSize);
            }
            else
                return new Rectangle(Position.ContentBounds.X + 1, (int) y, 0, 0);
        }
    }

	/// <summary>
	///     Obtiene el avatar de un mensaje
	/// </summary>
	private Sprites.SpriteDefinition? GetAvatar(MobileMessage message)
    {
        if (message.Sender is not null)
            return message.Sender.Avatar;
        else
            return null;
    }

	/// <summary>
	///     Dibuja el icono de "escribiendo"
	/// </summary>
	private void DrawWritting(Scenes.Rendering.RenderingManager renderingManager, Rectangle bounds, GameContext gameContext)
    {
        if (SpriteWriting is not null)
        {
            TweenResult<float> result = TweenCalculator.CalculateFloat(_typingElapsed, 2, 0, 1, Tools.MathTools.Easing.EasingFunctionsHelper.EasingType.Linear);
            Rectangle position = new(bounds.X, bounds.Y, (int) Math.Min(bounds.Width, SpriteWriting.GetSize().Width), AvatarSize);

                // Dibuja la textura
                if (SpriteWriting is not null)
                    renderingManager.SpriteRenderer.Draw(SpriteWriting, position, Vector2.Zero, 0, Color.White * result.Value);
                // Incrementa el tiempo pasado o vuelve a empezar
                if (result.Progress >= 0.99f)
                    _typingElapsed = 0;
                else
                    _typingElapsed += gameContext.DeltaTime;
        }
    }

    /// <summary>
    ///     Dibuja el fondo
    /// </summary>
	private void DrawBackground(Scenes.Rendering.RenderingManager renderingManager, MobileSender sender, Rectangle bounds)
	{
        if (sender.SpriteBackground is not null)
            renderingManager.SpriteRenderer.Draw(sender.SpriteBackground, bounds, Vector2.Zero, 0, sender.BackgroundColor);
        else
		    renderingManager.FiguresRenderer.DrawRectangle(bounds, sender.BackgroundColor);
	}

    /// <summary>
    ///     Dibuja las líneas del mensaje
    /// </summary>
    private void DrawMessage(Scenes.Rendering.RenderingManager renderingManager, MobileMessage message, Rectangle bounds)
    {
        if (Font is not null)
        {
            int y = bounds.Y + PaddingMessage.Top;
            int height = (int) Font.GetLineSpacing();

                // Escribe el nombre del remitente
                if (message.Sender.ShowName)
                {
                    Vector2 size = Font.MeasureString(message.Sender.Name);

                        // Dibuja el nombre
                        renderingManager.SpriteTextRenderer.DrawString(Font, message.Sender.Name, 
                                                                       new Vector2(bounds.X + PaddingMessage.Left, y), message.Sender.NameForecolor);
                        // Incrementa la posición
                        y += (int) size.Y + 2;
                }
                // Escribe las líneas
                foreach (string line in message.Lines)
                {
                    // Escribe el texto
                    renderingManager.SpriteTextRenderer.DrawString(Font, line, new Vector2(bounds.X + PaddingMessage.Left, y), message.Sender.Forecolor);
                    // Pasa a la siguiente línea
                    y += height;
                }
        }
    }

    /// <summary>
    ///     Nombre de la fuente
    /// </summary>
    public Sprites.SpriteTextDefinition? Font { get; set; }

    /// <summary>
    ///     Altura del mensaje en pantalla
    /// </summary>
    public Sprites.SpriteDefinition? SpriteWriting { get; set; }

    /// <summary>
    ///     Tamaño del avatar en pixels
    /// </summary>
    public int AvatarSize { get; set; } = 32;

    /// <summary>
    ///     Espaciado de los mensajes
    /// </summary>
    public float MessageSpacing { get; set; } = 0.02f;

    /// <summary>
    ///     Padding del mensaje de texto
    /// </summary>
    public UiMargin PaddingMessage { get; set; } = new(5, 5, 15, 5);
}
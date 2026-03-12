using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Tools.Tween;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI.UserInterface.MobileChats;

/// <summary>
///		Componente para mostrar una conversación de móvil
/// </summary>
public class UiMobileChat(Bau.Libraries.BauGame.Engine.Scenes.Layers.AbstractUserInterfaceLayer layer, UiPosition position) : UiElement(layer, position)
{
    // Variables privadas
    private bool _initialized = false;
    private List<MobileSender> _senders = [];
    private List<MobileMessage> _messages = [];
    private SpriteFont? _spriteFont;
    private float _typingElapsed = 8f;
    private Texture2D? _typingTexture = null;

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
                MobileMessage message = new(participant, text);

                    // Calcula el tiempo que va a tardar en mostrarse
                    message.TargetY = Position.ScreenPaddedBounds.Bottom;
                    message.TimeToShow = timeToShow + ComputePreviousTime();
                    message.Visible = false;
                    // Prepara el mensaje
                    PrepareMessage(message);
                    // Añade el mensaje a la lista
                    _messages.Add(message);
            }

        // Calcula el tiempo anterior a este mensaje
        float ComputePreviousTime()
        {
            float total = 0;

                // Acumula el tiempo a mostrar de tods los mensajes
                foreach (MobileMessage message in _messages)
                    if (message.TimeToShow > 0)
                        total += message.TimeToShow;
                // Devuelve el tiempo calculado
                return total;
        }
    }
        
    /// <summary>
    ///     Recalcula las posiciones de los mensajes
    /// </summary>
    private void RecalculateMessagePositions()
    {
        if (_messages.Count > 0)
        {
            int lastMessageIndex = GetLastVisibleMessage();
            float currentY = Position.ScreenPaddedBounds.Height - 0.05f;

                // Quita los mensajes que estén por encima del límite superior
                for (int index = _messages.Count - 1; index >= 0; index--)
                    if (_messages[index].TargetY < Position.ScreenPaddedBounds.Top)
                        _messages.RemoveAt(index);
                // Si el último mensaje visible ha terminado su tiempo de espera, el siguiente pasa a ser el último visible
                if (lastMessageIndex < _messages.Count - 1)
                {
                    if (_messages[lastMessageIndex].TimeToShow == 0)
                        _messages[++lastMessageIndex].Visible = true;
                }
                // Calculamos la posición objetivo
                for (int index = lastMessageIndex; index >= 0; index--)
                {
                    // Quita el alto del mensaje a la posición actual                
                    currentY -= _messages[index].Height;
                    // Cambia la coordenada de destino y la visibilidad
                    _messages[index].Visible = true;
                    _messages[index].TargetY = currentY;
                    // Quita el alto de la coordenada inicial
                    currentY -= MessageSpacing;
                }
        }

        // Obtiene el último mensaje visible
        int GetLastVisibleMessage()
        {
            // Busca el último mensaje visible
            for (int index = _messages.Count - 1; index >= 0; index--)
                if (_messages[index].Visible)
                    return index;
            // Si ha llegado hasta aquí es porque no ha encontrado ningún mensaje visible
            return 0;
        }
    }

    /// <summary>
    ///     Recalcula las posiciones de los mensajes
    /// </summary>
    private void PrepareMessage(MobileMessage message)
    {
        if (_spriteFont is not null)
        {
            // Divide el texto en líneas
            message.Lines.Clear();
            message.Lines.AddRange(new Helpers.StringFontHelper().WrapText(_spriteFont, message.Text, 1, GetMaxBubbleWidth()));
            // Calcula la altura
            message.Height = message.Lines.Count * _spriteFont.LineSpacing * 0.8f + MessagePadding * 2;
        }
    }

    /// <summary>
    ///     Obtiene el ancho máximo de un mensaje
    /// </summary>
    private int GetMaxBubbleWidth() => (int) (MaxMessageWidth * Position.ScreenPaddedBounds.Width) - (int) AvatarSize - 5;

    /// <summary>
    ///     Calcula los límites del control
    /// </summary>
    protected override void ComputeScreenComponentBounds()
	{
	    // TODO: calcular el tamaño
        // Cuando se cambia el tamaño, cambia los datos de los mensajes
        foreach (MobileMessage message in _messages)
            PrepareMessage(message);
	}

    /// <summary>
    ///     Actualiza los datos
    /// </summary>
	public override void Update(GameContext gameContext)
	{
        // Inicializa los datos
        if (!_initialized)
        {
            // Carga la fuente
            if (!string.IsNullOrWhiteSpace(Font))
                _spriteFont = Layer.Scene.LoadSceneAsset<SpriteFont>(Font);
            // Indica que se ha inicializado
            _initialized = true;
        }
        // Recalcula las posiciones
        RecalculateMessagePositions();
        // Animar mensajes (suavizado hacia posición objetivo)
        foreach (MobileMessage message in _messages)
        {
            // Quita el tiempo pendiente de mostrar
            if (message.TimeToShow > 0)
                message.TimeToShow -= gameContext.DeltaTime;
            // Mueve el mensaje si es necesario
            if (message.TargetY != message.CurrentY && message.Visible)
            {
                float difference = message.TargetY - message.CurrentY;

                    // Calcula la posición nueva
                    message.CurrentY += difference * MessageSlideSpeed * gameContext.DeltaTime;
            }
        }
    }

    /// <summary>
    ///     Dibuja los mensajes
    /// </summary>
	public override void Draw(Camera2D camera, GameContext gameContext)
	{
        // Dibuja los datos dependiendo del estilo
        // TODO: dibuja los datos del estilo
        // Dibuja los mensajes
        foreach (MobileMessage message in _messages)
            if (message.Visible)
            {
                int width = GetMaxBubbleWidth();
                Rectangle bounds = new(GetMessageX(message, width), (int) message.CurrentY, width, (int) message.Height);

                    // Dibuja el avatar
                    DrawAvatar(camera, message.Sender, 
                               new Rectangle((int) (bounds.X - AvatarSize - 5), (int) (0.5f * (bounds.Height - AvatarSize)), (int) AvatarSize, (int) AvatarSize));
                    // Dibuja el rectángulo de fondo
                    camera.SpriteBatchController.DrawRectangle(bounds, message.Sender.BackgroundColor);
                    // Dibuja el contenido del mensaje
                    if (message.TimeToShow > 0)
                        DrawWritting(camera, bounds, gameContext);
                    else
                    {
                        // Dibuja el texto del mensaje
                        DrawMessage(camera, message, bounds);
                        // Vacía el tiempo del indicador de escritura porque hemos escrito un mensaje
                        _typingElapsed = 0;
                    }
            }

        // Obtiene la coordenada X donde se debe mostrar un mensaje
        int GetMessageX(MobileMessage message, int messageWidth)
        {
            if (message.Sender.IsPlayer)
                return Position.ScreenPaddedBounds.Width - messageWidth;
            else
                return Position.ScreenPaddedBounds.Left;
        }
    }

	/// <summary>
	///     Dibuja el icono de "escribiendo"
	/// </summary>
	private void DrawWritting(Camera2D camera, Rectangle bounds, GameContext gameContext)
    {
        if (_typingTexture is not null)
        {
            TweenResult<float> result = TweenCalculator.CalculateFloat(_typingElapsed, 2, 0, 1, TweenCalculator.EaseType.Linear);

                // Dibuja la textura
                camera.SpriteBatchController.Draw(_typingTexture, bounds, Color.White * result.Value);
                // Incrementa el tiempo pasado o vuelve a empezar
                if (result.Progress >= 0.99f)
                    _typingElapsed = 0;
                else
                    _typingElapsed += gameContext.DeltaTime;
        }
    }

    /// <summary>
    ///     Dibuja las líneas del mensaje
    /// </summary>
    private void DrawMessage(Camera2D camera, MobileMessage message, Rectangle bounds)
    {
        if (_spriteFont is not null)
        {
            int y = bounds.Y;
            int height = (int) (_spriteFont.LineSpacing * 0.8f);

                // Escribe las líneas
                foreach (string line in message.Lines)
                {
                    // Escribe el texto
                    camera.SpriteBatchController.DrawString(_spriteFont, line, new Vector2(bounds.X, y), message.Sender.Forecolor);
                    // Pasa a la siguiente línea
                    y += height;
                }
        }
    }
    
    /// <summary>
    ///     Dibuja el avatar
    /// </summary>
    private void DrawAvatar(Camera2D camera, MobileSender sender, Rectangle bounds)
    {
    /*
        if (avatar != null)
        {
            // Crear máscara circular para el avatar
            _spriteBatch.Draw(avatar, new Rectangle(x, y, size, size), Color.White);
        }
        else
        {
            // Avatar placeholder (círculo con iniciales)
            var rect = new Rectangle(x, y, size, size);
            _spriteBatch.Draw(_pixelTexture, rect, new Color(100, 100, 100));
        }
        */
    }

    /// <summary>
    ///     Nombre de la fuente
    /// </summary>
    public string? Font { get; set; }

    /// <summary>
    ///     Tamaño del avatar en pixels
    /// </summary>
    public float AvatarSize { get; set; } = 20;

    /// <summary>
    ///     Padding de los mensajes
    /// </summary>
    public float MessagePadding { get; set; } = 0.02f;

    /// <summary>
    ///     Espaciado entre mensajes
    /// </summary>
    public float MessageSpacing { get; set; }= 0.015f;

    /// <summary>
    ///     Ancho máximo de un mensaje (relativo)
    /// </summary>
    public float MaxMessageWidth { get; set; } = 0.85f;

    /// <summary>
    ///     Velocidad de movimiento de un mensaje
    /// </summary>
    public float MessageSlideSpeed = 3f;
}
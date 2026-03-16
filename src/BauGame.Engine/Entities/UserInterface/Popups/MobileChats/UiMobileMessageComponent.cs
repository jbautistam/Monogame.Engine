using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI.UserInterface.MobileChats;

/// <summary>
///		Elemento de un mensaje
/// </summary>
internal class UiMobileMessageComponent(UiMobileChat mobileChat) : UiElement(mobileChat.Layer, new UiPosition(0, 0, 0, 0))
{
	protected override void ComputeScreenComponentBounds()
	{
	throw new NotImplementedException();
	}

	public override void Update(GameContext gameContext)
	{
	throw new NotImplementedException();
	}

	public override void Draw(Camera2D camera, GameContext gameContext)
	{
	throw new NotImplementedException();
	}
        
    private float CalculateBubbleWidth(string text)
    {
        int maxWidthPx = (int)(_maxBubbleWidth * _screenWidth);
        var lines = WrapText(_font, text, maxWidthPx - 20);
            
        float maxLineWidth = 0;
        foreach (var line in lines)
        {
            var size = _font.MeasureString(line) * 0.8f;
            if (size.X > maxLineWidth) maxLineWidth = size.X;
        }
            
        float widthWithPadding = (maxLineWidth + 40) / _screenWidth;
        return Math.Min(widthWithPadding, _maxBubbleWidth);
    }

        
    private void DrawMessage(ChatMessage msg, GameTime gameTime)
    {
        float bubbleWidthRel = CalculateBubbleWidth(msg.Text);
        float bubbleHeightRel = CalculateBubbleHeight(msg.Text);
            
        int bubbleWidth = (int)(bubbleWidthRel * _screenWidth);
        int bubbleHeight = (int)(bubbleHeightRel * _screenHeight);
        int avatarSize = (int)(_avatarSize * Math.Min(_screenWidth, _screenHeight));
            
        // Calcular posición X según si es jugador o NPC
        int xPos;
        int avatarX;
            
        if (msg.IsPlayer)
        {
            // Jugador: alineado a la derecha
            int panelRight = (int)((_panelBounds.X + _panelBounds.Width) * _screenWidth);
            xPos = panelRight - bubbleWidth - (int)(0.02f * _screenWidth);
            avatarX = xPos - avatarSize - (int)(0.01f * _screenWidth);
        }
        else
        {
            // NPC: alineado a la izquierda
            int panelLeft = (int)(_panelBounds.X * _screenWidth);
            xPos = panelLeft + avatarSize + (int)(0.02f * _screenWidth);
            avatarX = panelLeft + (int)(0.01f * _screenWidth);
        }
            
        int yPos = (int)(msg.CurrentY * _screenHeight);
            
        // Dibujar avatar (círculo recortado)
        DrawAvatar(msg.Sender.AvatarTexture, avatarX, yPos + bubbleHeight / 2 - avatarSize / 2, avatarSize);
            
        // Color de burbuja
        Color bubbleColor = msg.IsPlayer ? PLAYER_BUBBLE_COLOR : NPC_BUBBLE_COLOR;
        bubbleColor = Color.Multiply(bubbleColor, msg.Opacity);
            
        // Dibujar burbuja con sombra sutil
        DrawBubble(xPos, yPos, bubbleWidth, bubbleHeight, bubbleColor, msg.IsPlayer);
            
        // Dibujar texto
        string wrappedText = string.Join("\n", WrapText(_font, msg.Text, bubbleWidth - 20));
        Vector2 textPos = new Vector2(xPos + 10, yPos + 10);
        Color textColor = Color.Multiply(msg.IsPlayer ? PLAYER_TEXT_COLOR : NPC_TEXT_COLOR, msg.Opacity);
            
        _spriteBatch.DrawString(_font, wrappedText, textPos, textColor, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
            
        // Dibujar hora pequeña
        string timeStr = msg.Timestamp.ToString("HH:mm");
        Vector2 timeSize = _font.MeasureString(timeStr) * 0.6f;
        Vector2 timePos = new Vector2(xPos + bubbleWidth - timeSize.X - 5, yPos + bubbleHeight - timeSize.Y - 2);
        _spriteBatch.DrawString(_font, timeStr, timePos, Color.Multiply(Color.Gray, msg.Opacity), 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            
        // Doble check para mensajes del jugador (estado "enviado")
        if (msg.IsPlayer)
        {
            DrawCheckMarks(xPos + bubbleWidth - 15, yPos + bubbleHeight - 12, Color.Multiply(Color.Gray, msg.Opacity));
        }
    }
    
    private void DrawAvatar(Texture2D avatar, int x, int y, int size)
    {
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
    }
        
    private void DrawBubble(int x, int y, int width, int height, Color color, bool isPlayer)
    {
        // Escalar textura de burbuja
        var destRect = new Rectangle(x, y, width, height);
            
        // Sombra sutil
        var shadowRect = new Rectangle(x + 2, y + 2, width, height);
        _spriteBatch.Draw(_bubbleTexture, shadowRect, Color.Multiply(Color.Black, 0.1f));
            
        // Burbuja principal
        _spriteBatch.Draw(_bubbleTexture, destRect, color);
            
        // Pico de la burbuja (triángulo pequeño)
        DrawBubbleTail(x, y, height, color, isPlayer);
    }
        
    private void DrawBubbleTail(int bubbleX, int bubbleY, int bubbleHeight, Color color, bool pointingRight)
    {
        int tailSize = 10;
        int tailY = bubbleY + bubbleHeight / 2;
            
        if (pointingRight)
        {
            // Pico apuntando a la derecha (jugador)
            var tailRect = new Rectangle(bubbleX + (int)(_maxBubbleWidth * _screenWidth) - 5, tailY - tailSize/2, tailSize, tailSize);
            // Simplificado: pequeño triángulo dibujado con líneas o polígonos
        }
        else
        {
            // Pico apuntando a la izquierda (NPC)
            var tailRect = new Rectangle(bubbleX - tailSize + 5, tailY - tailSize/2, tailSize, tailSize);
        }
    }
        
    private void DrawTypingIndicator(GameTime gameTime)
    {
        if (_currentTypingNPC == null) return;
            
        int avatarSize = (int)(_avatarSize * Math.Min(_screenWidth, _screenHeight));
        int panelLeft = (int)(_panelBounds.X * _screenWidth);
        int avatarX = panelLeft + (int)(0.01f * _screenWidth);
            
        // Posición debajo del último mensaje o en una posición fija
        float lastMsgY = _messages.Count > 0 ? _messages.Last().TargetY : 0.9f;
        int yPos = (int)(lastMsgY * _screenHeight) - avatarSize - 10;
            
        // Dibujar avatar del NPC que escribe
        DrawAvatar(_currentTypingNPC.AvatarTexture, avatarX, yPos, avatarSize);
            
        // Dibujar burbuja de escritura
        int bubbleWidth = 80;
        int bubbleHeight = 40;
        int bubbleX = avatarX + avatarSize + (int)(0.01f * _screenWidth);
            
        var bubbleRect = new Rectangle(bubbleX, yPos + avatarSize/2 - bubbleHeight/2, bubbleWidth, bubbleHeight);
        _spriteBatch.Draw(_bubbleTexture, bubbleRect, NPC_BUBBLE_COLOR);
            
        // Animar puntos de escritura
        float bounce = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * _typingBounceSpeed);
        int dotOffset = (int)(bounce * 3);
            
        var dotsRect = new Rectangle(bubbleX + 20, bubbleRect.Y + 10 + dotOffset, 40, 20);
        _spriteBatch.Draw(_typingDotsTexture, dotsRect, TYPING_INDICATOR_COLOR);
    }
        
    private void DrawCheckMarks(int x, int y, Color color)
    {
        // Doble check estilo WhatsApp (simplificado como dos líneas)
        int size = 8;
        // Dibujar dos V superpuestas
        DrawLine(x, y, x + size/2, y + size, color, 1);
        DrawLine(x + size/2, y + size, x + size, y, color, 1);
        DrawLine(x + 2, y, x + size/2 + 2, y + size, color, 1);
        DrawLine(x + size/2 + 2, y + size, x + size + 2, y, color, 1);
    }
        
    private void DrawLine(int x1, int y1, int x2, int y2, Color color, int thickness)
    {
        // Implementación simplificada de línea usando rectángulos rotados
        // En producción, usar una textura de línea o SpriteBatch con rotación
    }

    public float CurrentY { get; set; }

    public float TargetY { get; set; }

    public bool IsNew { get; set; }
}
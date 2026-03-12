using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace UI.UserInterface.ComicBubbles;

public class UiComicBubble(AbstractUserInterfaceLayer layer, UiPosition position) : UiElement(layer, position)
{
    // Texturas separadas
    private Texture2D _bubbleTexture;   // Fondo escalable (cuerpo del bocadillo)
    private Texture2D _tailTexture;     // Cola fija (opcional)
    private SpriteFont _font;
    private GraphicsDevice _graphicsDevice;
    private SpriteBatch _spriteBatch;
        
    // Configuración de posición
    private float _positionX, _positionY;   // Posición del anclaje (0-1)
    private AnchorPoint _anchor;             // Anclaje del bocadillo completo
    private TailPosition _tailPosition;      // Dónde dibujar la cola
        
    // Dimensiones
    private float _maxWidth;     // Ancho máximo del bocadillo (0-1)
    private float _minWidth;     // Ancho mínimo del bocadillo (0-1)
    private float _padding;      // Padding interno del bocadillo (píxeles)
        
    // Offset de la cola (ajuste fino de posición)
    private Vector2 _tailOffset;
        
    // Alineación del texto
    public HorizontalAlignment TextHorizontalAlign { get; set; } = HorizontalAlignment.Center;
    public VerticalAlignment TextVerticalAlign { get; set; } = VerticalAlignment.Center;
        
    // Estilo
    public Color TextColor { get; set; } = Color.Black;
    public Color BubbleColor { get; set; } = Color.White;
    public Color TailColor { get; set; } = Color.White;
    public float TextScale { get; set; } = 1.0f;
    public float LineSpacing { get; set; } = 1.0f;
        
    // Animaciones
    private float _currentOpacity = 1f;
    private float _targetOpacity = 1f;
    private float _fadeSpeed = 5f;
    private float _scaleProgress = 1f;
    private float _targetScale = 1f;
    private float _scaleSpeed = 8f;
        
    // Typewriter
    private float _typewriterProgress = 1f;
    private bool _isTyping = false;
    private float _typewriterSpeed = 30f;
        
    // Estado
    private string _fullText = "";
    //private List<string> _wrappedLines = new List<string>();
    private Vector2 _contentSize;
    private Vector2 _bubbleSize;
    private int _screenWidth, _screenHeight;
    private bool _needsRecalculation = true;
        
    public enum HorizontalAlignment { Left, Center, Right }
    public enum VerticalAlignment { Top, Center, Bottom }
        
    public enum AnchorPoint 
    { 
        TopLeft, TopCenter, TopRight,
        MiddleLeft, Center, MiddleRight,
        BottomLeft, BottomCenter, BottomRight 
    }
        
    public enum TailPosition
    {
        None,
        BottomLeft, BottomCenter, BottomRight,
        TopLeft, TopCenter, TopRight,
        LeftTop, LeftCenter, LeftBottom,
        RightTop, RightCenter, RightBottom
    }
        
    private void Initialize(GraphicsDevice graphicsDevice, Texture2D bubbleTexture, Texture2D tailTexture, SpriteFont font)
    {
        _graphicsDevice = graphicsDevice;
        _bubbleTexture = bubbleTexture;
        _tailTexture = tailTexture;
        _font = font;
        _spriteBatch = new SpriteBatch(graphicsDevice);
            
        _maxWidth = 0.8f;
        _minWidth = 0.1f;
        _positionX = 0.5f;
        _positionY = 0.5f;
        _anchor = AnchorPoint.Center;
        _tailPosition = TailPosition.None;
        _padding = 15f;
        _tailOffset = Vector2.Zero;
    }

	protected override void ComputeScreenComponentBounds()
	{
	throw new NotImplementedException();
	}
        
    private void RecalculateBubbleSize()
    {
        if (!_needsRecalculation) return;
        if (string.IsNullOrEmpty(_fullText)) return;
            
        // Área disponible para texto (bocadillo - padding)
        float maxContentWidth = (_maxWidth * _screenWidth) - (_padding * 2);
        float minContentWidth = (_minWidth * _screenWidth) - (_padding * 2);
            
        maxContentWidth = Math.Max(maxContentWidth, 50f);
        minContentWidth = Math.Max(minContentWidth, 30f);
            
        // Encontrar ancho óptimo para el texto
        float bestWidth = FindOptimalWidth(_fullText, minContentWidth, maxContentWidth);
            
            
        // Dimensiones del contenido
        float contentWidth = 0f;
        float contentHeight = 0f;
        float lineHeight = _font.LineSpacing * LineSpacing * TextScale;
            
        foreach (string line in new Helpers.StringFontHelper().WrapText(_font, _fullText, TextScale, bestWidth))
        {
            Vector2 size = _font.MeasureString(line) * TextScale;
            contentWidth = Math.Max(contentWidth, size.X);
            contentHeight += lineHeight;
        }
            
        contentHeight -= (lineHeight - _font.LineSpacing * TextScale);
            
        _contentSize = new Vector2(contentWidth, contentHeight);
            
        // Tamaño del bocadillo = contenido + padding
        _bubbleSize = new Vector2(
            contentWidth + (_padding * 2),
            contentHeight + (_padding * 2)
        );
            
        _needsRecalculation = false;
    }
        
    /// <summary>
    ///     Busca el ancho óptimo para mostrar un texto
    /// </summary>
    private float FindOptimalWidth(string text, float minWidth, float maxWidth)
    {
        if (string.IsNullOrWhiteSpace(text))
            return minWidth;
        else
        {
            float minRequired = GetMinRequired(text, minWidth);
            
            // Si cabe en una línea
            Vector2 fullSize = _font.MeasureString(text) * TextScale;
            if (fullSize.X <= maxWidth) return fullSize.X;
            
            // Buscar óptimo
            float best = maxWidth;
            float bestArea = float.MaxValue;
            
            for (float test = minRequired; test <= maxWidth; test += 10f)
            {
                List<string> lines = new Helpers.StringFontHelper().WrapText(_font, text, 1, test);
                float area = test * lines.Count;
                if (area < bestArea)
                {
                    bestArea = area;
                    best = test;
                }
                if (lines.Count <= 2) break;
            }
            
            return best;
        }

        // Obtiene el ancho mínimo requerido para mostrar la palabra más larga
        float GetMinRequired(string text, float minWidth)
        {
            float minRequired = 0f;

                // Recorre las palabras buscando la palabra más larga
                foreach (string word in text.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    minRequired = Math.Max(minRequired, (_font.MeasureString(word) * TextScale).X);
                // Devuelve el valor mínimo
                return Math.Max(minRequired, minWidth);
        }
    }

	public override void Update(GameContext gameContext)
	{
        RecalculateBubbleSize();
            
        // Fade
        if (Math.Abs(_currentOpacity - _targetOpacity) > 0.001f)
        {
            _currentOpacity += (_targetOpacity - _currentOpacity) * _fadeSpeed * gameContext.DeltaTime;
            _currentOpacity = MathHelper.Clamp(_currentOpacity, 0f, 1f);
        }
            
        // Scale
        if (_scaleProgress < _targetScale)
        {
            _scaleProgress += _scaleSpeed * gameContext.DeltaTime;
            if (_scaleProgress > _targetScale) _scaleProgress = _targetScale;
        }
            
        // Typewriter
        if (_isTyping && _typewriterProgress < 1f)
        {
            _typewriterProgress += _typewriterSpeed * gameContext.DeltaTime / _fullText.Length;
            if (_typewriterProgress >= 1f)
            {
                _typewriterProgress = 1f;
                _isTyping = false;
            }
        }
	}

	public override void Draw(Camera2D camera, GameContext gameContext)
	{
        if (!string.IsNullOrWhiteSpace(_fullText))
        {
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp);
            
        // Calcular rectángulos
        Rectangle bubbleRect = CalculateBubbleRectangle();
        Rectangle tailRect = CalculateTailRectangle(bubbleRect);
            
        // Aplicar animación de escala (desde el centro)
        if (_scaleProgress < 1f)
        {
            bubbleRect = ScaleRectangle(bubbleRect, _scaleProgress);
            if (tailRect.Width > 0)
                tailRect = ScaleRectangle(tailRect, _scaleProgress);
        }
            
        Color bubbleTint = BubbleColor;
        bubbleTint.A = (byte)(_currentOpacity * 255);
        Color tailTint = TailColor;
        tailTint.A = (byte)(_currentOpacity * 255);
            
        // Dibujar cola PRIMERO (detrás del bocadillo si se solapan)
        if (_tailTexture != null && tailRect.Width > 0)
            _spriteBatch.Draw(_tailTexture, tailRect, tailTint);
        // Dibujar bocadillo
        _spriteBatch.Draw(_bubbleTexture, bubbleRect, bubbleTint);
        // Dibujar texto
        DrawText(bubbleRect, new Helpers.StringFontHelper().WrapText(_font, _fullText, TextScale, Position.ScreenPaddedBounds.Width));
            
        _spriteBatch.End();
        }
	}
        
    private Rectangle CalculateBubbleRectangle()
    {
        int w = (int)_bubbleSize.X;
        int h = (int)_bubbleSize.Y;
        int ax = (int)(_positionX * _screenWidth);
        int ay = (int)(_positionY * _screenHeight);
            
        int x = ax, y = ay;
            
        switch (_anchor)
        {
            case AnchorPoint.TopLeft:      x = ax; y = ay; break;
            case AnchorPoint.TopCenter:    x = ax - w/2; y = ay; break;
            case AnchorPoint.TopRight:     x = ax - w; y = ay; break;
            case AnchorPoint.MiddleLeft:   x = ax; y = ay - h/2; break;
            case AnchorPoint.Center:       x = ax - w/2; y = ay - h/2; break;
            case AnchorPoint.MiddleRight:  x = ax - w; y = ay - h/2; break;
            case AnchorPoint.BottomLeft:   x = ax; y = ay - h; break;
            case AnchorPoint.BottomCenter: x = ax - w/2; y = ay - h; break;
            case AnchorPoint.BottomRight:  x = ax - w; y = ay - h; break;
        }
            
        return new Rectangle(x, y, w, h);
    }
        
    private Rectangle CalculateTailRectangle(Rectangle bubbleRect)
    {
        if (_tailTexture == null || _tailPosition == TailPosition.None)
            return Rectangle.Empty;
            
        int tailW = _tailTexture.Width;
        int tailH = _tailTexture.Height;
        int x = 0, y = 0;
            
        switch (_tailPosition)
        {
            // Colas inferiores
            case TailPosition.BottomLeft:
                x = bubbleRect.Left + (int)_tailOffset.X;
                y = bubbleRect.Bottom - (tailH / 2) + (int)_tailOffset.Y;
                break;
            case TailPosition.BottomCenter:
                x = bubbleRect.Left + (bubbleRect.Width / 2) - (tailW / 2) + (int)_tailOffset.X;
                y = bubbleRect.Bottom - (tailH / 2) + (int)_tailOffset.Y;
                break;
            case TailPosition.BottomRight:
                x = bubbleRect.Right - tailW + (int)_tailOffset.X;
                y = bubbleRect.Bottom - (tailH / 2) + (int)_tailOffset.Y;
                break;
                    
            // Colas superiores
            case TailPosition.TopLeft:
                x = bubbleRect.Left + (int)_tailOffset.X;
                y = bubbleRect.Top - (tailH / 2) + (int)_tailOffset.Y;
                break;
            case TailPosition.TopCenter:
                x = bubbleRect.Left + (bubbleRect.Width / 2) - (tailW / 2) + (int)_tailOffset.X;
                y = bubbleRect.Top - (tailH / 2) + (int)_tailOffset.Y;
                break;
            case TailPosition.TopRight:
                x = bubbleRect.Right - tailW + (int)_tailOffset.X;
                y = bubbleRect.Top - (tailH / 2) + (int)_tailOffset.Y;
                break;
                    
            // Colas izquierdas
            case TailPosition.LeftTop:
                x = bubbleRect.Left - (tailW / 2) + (int)_tailOffset.X;
                y = bubbleRect.Top + (int)_tailOffset.Y;
                break;
            case TailPosition.LeftCenter:
                x = bubbleRect.Left - (tailW / 2) + (int)_tailOffset.X;
                y = bubbleRect.Top + (bubbleRect.Height / 2) - (tailH / 2) + (int)_tailOffset.Y;
                break;
            case TailPosition.LeftBottom:
                x = bubbleRect.Left - (tailW / 2) + (int)_tailOffset.X;
                y = bubbleRect.Bottom - tailH + (int)_tailOffset.Y;
                break;
                    
            // Colas derechas
            case TailPosition.RightTop:
                x = bubbleRect.Right - (tailW / 2) + (int)_tailOffset.X;
                y = bubbleRect.Top + (int)_tailOffset.Y;
                break;
            case TailPosition.RightCenter:
                x = bubbleRect.Right - (tailW / 2) + (int)_tailOffset.X;
                y = bubbleRect.Top + (bubbleRect.Height / 2) - (tailH / 2) + (int)_tailOffset.Y;
                break;
            case TailPosition.RightBottom:
                x = bubbleRect.Right - (tailW / 2) + (int)_tailOffset.X;
                y = bubbleRect.Bottom - tailH + (int)_tailOffset.Y;
                break;
        }
            
        return new Rectangle(x, y, tailW, tailH);
    }
        
    private Rectangle ScaleRectangle(Rectangle rect, float scale)
    {
        int newW = (int)(rect.Width * scale);
        int newH = (int)(rect.Height * scale);
        int newX = rect.X + (rect.Width - newW) / 2;
        int newY = rect.Y + (rect.Height - newH) / 2;
        return new Rectangle(newX, newY, newW, newH);
    }
        
    private void DrawText(Rectangle bubbleRect, List<string> lines)
    {
        int contentX = bubbleRect.X + (int)_padding;
        int contentY = bubbleRect.Y + (int)_padding;
        int contentW = bubbleRect.Width - (int)(_padding * 2);
        int contentH = bubbleRect.Height - (int)(_padding * 2);
            
        float lineHeight = _font.LineSpacing * LineSpacing * TextScale;
        float totalTextH = lines.Count * lineHeight;
        totalTextH -= (lineHeight - _font.LineSpacing * TextScale);
            
        // Posición Y inicial
        float startY;
        switch (TextVerticalAlign)
        {
            case VerticalAlignment.Top: startY = contentY; break;
            case VerticalAlignment.Center: startY = contentY + (contentH - totalTextH) / 2f; break;
            case VerticalAlignment.Bottom: startY = contentY + contentH - totalTextH; break;
            default: startY = contentY; break;
        }
            
        // Dibujar líneas (con typewriter)
        int visibleChars = _isTyping ? (int)(_fullText.Length * _typewriterProgress) : _fullText.Length;
        int charsDrawn = 0;
            
        for (int i = 0; i < lines.Count; i++)
        {
            string line = lines[i];
            int remaining = visibleChars - charsDrawn;
            if (remaining <= 0) break;
                
            int show = Math.Min(line.Length, remaining);
            string visible = line.Substring(0, show);
            charsDrawn += show;
                
            Vector2 size = _font.MeasureString(visible) * TextScale;
                
            // Posición X según alineación
            float x;
            switch (TextHorizontalAlign)
            {
                case HorizontalAlignment.Left: x = contentX; break;
                case HorizontalAlignment.Center: x = contentX + (contentW - size.X) / 2f; break;
                case HorizontalAlignment.Right: x = contentX + contentW - size.X; break;
                default: x = contentX; break;
            }
                
            float y = startY + (i * lineHeight);
            y = Math.Min(y, contentY + contentH - lineHeight);
                
            Color color = TextColor;
            color.A = (byte)(_currentOpacity * 255);
                
            _spriteBatch.DrawString(_font, visible, new Vector2(x, y), color, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0f);
        }
    }
}
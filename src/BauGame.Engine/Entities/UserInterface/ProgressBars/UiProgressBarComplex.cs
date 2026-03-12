using Bau.Libraries.BauGame.Engine.Entities.Common;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.ProgressBars;

/// <summary>
///     Barra de progreso compleja
/// </summary>
public class UiProgressBarComplex(Scenes.Layers.AbstractUserInterfaceLayer layer, UiPosition position) : UiElement(layer, position)
{
    /// <summary>
    ///     Modos de renderizado para la barra de progreso
    /// </summary>
    public enum ProgressFillMode
    {
        /// <summary>Escala la textura completa según el porcentaje</summary>
        Scale,
        /// <summary>Muestra solo una porción de la textura</summary>
        Clip,
        /// <summary>Escala manteniendo ratio de aspecto y recorta el exceso</summary>
        ScaleKeepAspect
    }

    /// <summary>
    ///     Dirección de llenado de la barra
    /// </summary>
    public enum FillDirection
    {
        LeftToRight,
        RightToLeft,
        TopToBottom,
        BottomToTop
    }
    /// <summary>
    ///     Alineación de la porción de la barra visible en modo Clip
    /// </summary>
    public enum ClipAlignment
    {
        Left,
        Right,
        Center,
        Top,
        Bottom
    }
    // Variables privadas
    private float _currentValue;

    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenBoundsSelf() 
    {
    }

    /// <summary>
    ///     Actualiza el contenido del elemento
    /// </summary>
    protected override void UpdateSelf(Managers.GameContext gameContext)
    {
        _currentValue = Animation.GetValue(gameContext.DeltaTime, _currentValue, Value);
    }

    /// <summary>
    ///     Dibuja el contenido del elemento
    /// </summary>
    public override void Draw(Camera2D camera, Managers.GameContext gameContext)
    {
        Background?.Draw(camera, Position.ContentBounds, Vector2.Zero, 0, BackgroundColor);
        if (_currentValue > 0.001f)
            DrawBar(camera);
    }

    /// <summary>
    ///     Dibuja la barra
    /// </summary>
    private void DrawBar(Camera2D camera)
    {
        Rectangle destination = GetDestination(GetIsHorizontal());

            // Dibuja la barra dependiendo del modo de relleno
            switch (FillMode)
            {
                case ProgressFillMode.Scale:
                        DrawFillScaled(camera, destination);
                    break;
                case ProgressFillMode.Clip:
                        DrawFillClipped(camera, destination, Position.ContentBounds);
                    break;
                case ProgressFillMode.ScaleKeepAspect:
                        DrawFillKeepAspect(camera, destination, Position.ContentBounds);
                    break;
            }

        // Obtiene el destino de dibujo
        Rectangle GetDestination(bool isHorizontal)
        {
            int fillWidth = isHorizontal ? (int) (Position.ContentBounds.Width * _currentValue) : Position.ContentBounds.Width;
            int fillHeight = !isHorizontal ? (int) (Position.ContentBounds.Height * _currentValue) : Position.ContentBounds.Height;
            int x = Position.ContentBounds.X;
            int y = Position.ContentBounds.Y;
            
                // Modifica x e y dependiendo de la dirección de relleno
                switch (Direction)
                {
                    case FillDirection.RightToLeft:
                            x = Position.ContentBounds.Right - fillWidth;
                        break;
                    case FillDirection.BottomToTop:
                            y = Position.ContentBounds.Bottom - fillHeight;
                        break;
                }
                // Devuelve el rectángulo de destino
                return new Rectangle(x, y, fillWidth, fillHeight);
        }
    }

    /// <summary>
    ///     Comprueba si la dirección es horizontal
    /// </summary>
    private bool GetIsHorizontal() => Direction == FillDirection.LeftToRight || Direction == FillDirection.RightToLeft;

    /// <summary>
    ///     Dibuja la barra de progreso escalado al contenido
    /// </summary>
    private void DrawFillScaled(Camera2D camera, Rectangle destination)
    {
        Bar?.Draw(camera, destination, Vector2.Zero, 0, BarColor);
    }
        
    private void DrawFillClipped(Camera2D camera, Rectangle destination, Rectangle fillArea)
    {
        ClipAlignment alignment = GetRealAlignment(alignment);
        Rectangle sourceRect;
        int visiblePixels;
        Rectangle actualDest;
            
        if (GetIsHorizontal())
        {
            int destWidth = (int)(fillArea.Width * _currentValue);
            destWidth = Math.Max(0, Math.Min(destWidth, fillArea.Width));
                
            visiblePixels = (int)(fillTexture.Width * _currentValue);
            visiblePixels = Math.Max(1, Math.Min(visiblePixels, fillTexture.Width));
                
            int sourceX;
            if (ClipConfig.InvertProgressDirection)
                sourceX = fillTexture.Width - visiblePixels;
            else
                sourceX = CalculateClipStartX(fillTexture.Width, visiblePixels, alignment);
                
            sourceRect = new Rectangle(sourceX + ClipConfig.PixelOffset, 0, visiblePixels, fillTexture.Height);
                
            int destX = fillArea.X;
            if (Direction == FillDirection.RightToLeft)
            {
                destX = fillArea.Right - destWidth;
            }
                
            actualDest = new Rectangle(destX, fillArea.Y, destWidth, fillArea.Height);
        }
        else
        {
            int destHeight = (int)(fillArea.Height * _currentValue);
            destHeight = Math.Max(0, Math.Min(destHeight, fillArea.Height));
                
            visiblePixels = (int) (fillTexture.Height * _currentValue);
            visiblePixels = Math.Max(1, Math.Min(visiblePixels, fillTexture.Height));
                
            int sourceY;
            if (ClipConfig.InvertProgressDirection)
                sourceY = fillTexture.Height - visiblePixels;
            else
                sourceY = CalculateClipStartY(fillTexture.Height, visiblePixels, alignment);
                
            sourceRect = new Rectangle(0, sourceY + ClipConfig.PixelOffset, fillTexture.Width, visiblePixels
            );
                
            int destY = fillArea.Y;
            if (Direction == FillDirection.BottomToTop)
                destY = fillArea.Bottom - destHeight;
                
            actualDest = new Rectangle(fillArea.X, destY, fillArea.Width, destHeight);
        }
            
        sourceRect = ClampToTexture(sourceRect, fillTexture);
        DrawClippedScaled(camera, sourceRect, actualDest);
    }
    
    /// <summary>
    ///     Calcula la X de inicio del clip
    /// </summary>
    private int CalculateClipStartX(int textureWidth, int visibleWidth, ClipAlignment alignment)
    {
        return alignment switch
                    {
                        ClipAlignment.Right => textureWidth - visibleWidth,
                        ClipAlignment.Center => (int) (0.5f * (textureWidth - visibleWidth)),
                        _ => 0
                    };
    }

    /// <summary>
    ///     Calcula la Y de inicio del clip
    /// </summary>
    private int CalculateClipStartY(int textureHeight, int visibleHeight, ClipAlignment alignment)
    {
        return alignment switch
                    {
                        ClipAlignment.Top => 0,
                        ClipAlignment.Bottom => textureHeight - visibleHeight,
                        ClipAlignment.Center => (int) (0.5f * (textureHeight - visibleHeight)),
                        _ => alignment == ClipAlignment.Left ? 0 : textureHeight - visibleHeight
                    };
    }
        
    /// <summary>
    ///     Obtiene la alineación real
    /// </summary>
    private ClipAlignment GetRealAlignment(ClipAlignment alignment)
    {
        // Obtiene la alineación cuando se ha definido como automática
        if (ClipConfig.AutoFlipWithDirection)
            switch (Direction)
            {
                case FillDirection.RightToLeft:
                        if (alignment == ClipAlignment.Left) 
                            return ClipAlignment.Right;
                        if (alignment == ClipAlignment.Right) 
                            return ClipAlignment.Left;
                    break;
                case FillDirection.BottomToTop:
                        if (alignment == ClipAlignment.Top) 
                            return ClipAlignment.Bottom;
                        if (alignment == ClipAlignment.Bottom) 
                            return ClipAlignment.Top;
                    break;
            }
        // Devuelve la alineación original
        return alignment;
    }
        
    private Rectangle ClampToTexture(Rectangle source, Texture2D texture)
    {
        int x = Math.Max(0, Math.Min(source.X, texture.Width - 1));
        int y = Math.Max(0, Math.Min(source.Y, texture.Height - 1));
        int w = Math.Min(source.Width, texture.Width - x);
        int h = Math.Min(source.Height, texture.Height - y);
            
        return new Rectangle(x, y, w, h);
    }
        
    private void DrawClippedScaled(Camera2D camera, Rectangle sourceRect, Rectangle destRect)
    {
        if (sourceRect.Width <= 0 || sourceRect.Height <= 0) return;
            
        float scale;
        if (GetIsHorizontal())
            scale = (float)destRect.Height / sourceRect.Height;
        else
            scale = (float)destRect.Width / sourceRect.Width;
            
        int drawWidth = (int)(sourceRect.Width * scale);
        int drawHeight = (int)(sourceRect.Height * scale);
            
        int drawX = destRect.X;
        int drawY = destRect.Y;
            
        if (GetIsHorizontal())
            drawY = destRect.Y + (destRect.Height - drawHeight) / 2;
        else
            drawX = destRect.X + (destRect.Width - drawWidth) / 2;
            
        var actualDest = new Rectangle(drawX, drawY, drawWidth, drawHeight);
            
        sb.Draw(_fill.Texture, actualDest, sourceRect, BarColor);
    }
        
    private void DrawFillKeepAspect(Camera2D camera, Rectangle destRect, Rectangle fillArea)
    {
        var fillTexture = _fill.Texture;
            
        float textureAspect = (float)fillTexture.Width / fillTexture.Height;
        float destAspect = (float)destRect.Width / destRect.Height;
            
        Rectangle sourceRect;
            
        if (textureAspect > destAspect)
        {
            int visibleWidth = (int)(fillTexture.Height * destAspect);
            sourceRect = new Rectangle(0, 0, (int)(visibleWidth * _currentValue), fillTexture.Height);
        }
        else
        {
            sourceRect = new Rectangle(
                0,
                (fillTexture.Height - (int)(fillTexture.Width / destAspect)) / 2,
                (int)(fillTexture.Width * _currentValue),
                (int)(fillTexture.Width / destAspect)
            );
        }
            
        sb.Draw(fillTexture, destRect, sourceRect, BarColor);
    }
        
    /// <summary>
    ///     Modo de relleno de la barra de progreso
    /// </summary>
    public ProgressFillMode FillMode { get; set; } = ProgressFillMode.Clip;

    /// <summary>
    ///     Modo de animación
    /// </summary>
    public ProgressAnimation Animation { get; set; } = new();

    /// <summary>
    ///     Configuración de corte de la barra de progreso
    /// </summary>
    public ClipSettings ClipConfig { get; set; } = new();

    /// <summary>
    ///     Dirección de relleno
    /// </summary>
    public FillDirection Direction { get; set; } = FillDirection.LeftToRight;

    /// <summary>
    ///     Valor de la barra de progreso
    /// </summary>
    public float Value { get; set; }

    /// <summary>
    ///     Sprite del fondo de la barra de progreso
    /// </summary>
    public SpriteDefinition? Background { get; set; }

    /// <summary>
    ///     Color de fondo
    /// </summary>
    public Color BackgroundColor { get; set; } = Color.White;

    /// <summary>
    ///     Sprite de la barra
    /// </summary>
    public SpriteDefinition? Bar { get; set; }

    /// <summary>
    ///     Color de la barra
    /// </summary>
    public Color BarColor { get; set; } = Color.White;
}

/*
    /// <summary>
    /// Factory para crear barras de progreso comunes
    /// </summary>
    public static class ProgressBarFactory
    {
        public static ProgressBar CreateHealthBar(ITextureAsset background, ITextureAsset fill, Rectangle bounds)
        {
            return new ProgressBar(background, fill, bounds)
            {
                FillColor = new Color(220, 50, 50),
                Animation = { Speed = 8f, Easing = ProgressAnimation.EasingType.Exponential },
                FillMode = ProgressFillMode.Clip,
                Padding = new Point(2, 2)
            };
        }
        
        public static ProgressBar CreateXPBar(ITextureAsset background, ITextureAsset fill, Rectangle bounds)
        {
            return new ProgressBar(background, fill, bounds)
            {
                FillColor = new Color(150, 50, 220),
                FillMode = ProgressFillMode.Clip,
                Padding = Point.Zero,
                Animation = { Speed = 3f }
            };
        }
        
        public static ProgressBar CreateLoadingBar(ITextureAsset background, ITextureAsset fill, Rectangle bounds)
        {
            return new ProgressBar(background, fill, bounds)
            {
                FillColor = new Color(50, 150, 255),
                FillMode = ProgressFillMode.Scale,
                Animation = { Enabled = false },
                Padding = new Point(4, 4)
            };
        }
        
        public static ProgressBar CreateManaBar(ITextureAsset background, ITextureAsset fill, Rectangle bounds)
        {
            return new ProgressBar(background, fill, bounds)
            {
                FillColor = new Color(50, 200, 255),
                Direction = FillDirection.BottomToTop,
                FillMode = ProgressFillMode.Clip,
                Padding = new Point(3, 3)
            };
        }
    }
*/
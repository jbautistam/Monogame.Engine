using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.UI.Brushes;

/// <summary>
/// Fondo con textura
/// </summary>
public class TextureBackground : IBackground
{
    /// <summary>
    /// Modos de escalado para texturas de fondo
    /// </summary>
    public enum TextureScaleMode
    {
        Stretch,    // Estira para llenar
        Tile,       // Repite en mosaico
        Center,     // Centra sin escalar
        Fit,        // Escala para que quepa completa
        Fill,       // Escala para cubrir todo (puede recortar)
        NineSlice   // Divide en 9 partes para UI escalable
    }

    public Texture2D Texture { get; set; }
    public Color Tint { get; set; } = Color.White;
        
    /// <summary>
    /// Modo de escalado de la textura
    /// </summary>
    public TextureScaleMode ScaleMode { get; set; } = TextureScaleMode.Stretch;

    public bool HasContent => Texture != null && !Texture.IsDisposed && Tint.A > 0;

    public TextureBackground(Texture2D texture)
    {
        Texture = texture;
    }

    public TextureBackground(Texture2D texture, Color tint) : this(texture)
    {
        Tint = tint;
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle bounds, Rectangle contentBounds)
    {
        if (!HasContent) return;

        switch (ScaleMode)
        {
            case TextureScaleMode.Stretch:
                // Estira la textura para llenar el área
                spriteBatch.Draw(Texture, bounds, Tint);
                break;

            case TextureScaleMode.Tile:
                // Repite la textura (tile)
                DrawTiled(spriteBatch, bounds);
                break;

            case TextureScaleMode.Center:
                // Centra la textura sin escalar
                DrawCentered(spriteBatch, bounds);
                break;

            case TextureScaleMode.Fit:
                // Escala manteniendo aspect ratio para que quepa completamente
                DrawFitted(spriteBatch, bounds, true);
                break;

            case TextureScaleMode.Fill:
                // Escala manteniendo aspect ratio cubriendo todo el área (puede recortar)
                DrawFitted(spriteBatch, bounds, false);
                break;

            case TextureScaleMode.NineSlice:
                // Divide la textura en 9 partes para escalado sin deformar bordes
                DrawNineSliced(spriteBatch, bounds);
                break;
        }
    }

    private void DrawTiled(SpriteBatch spriteBatch, Rectangle bounds)
    {
        int texWidth = Texture.Width;
        int texHeight = Texture.Height;
            
        for (int y = bounds.Top; y < bounds.Bottom; y += texHeight)
        {
            for (int x = bounds.Left; x < bounds.Right; x += texWidth)
            {
                int width = Math.Min(texWidth, bounds.Right - x);
                int height = Math.Min(texHeight, bounds.Bottom - y);
                    
                var sourceRect = new Rectangle(0, 0, width, height);
                var destRect = new Rectangle(x, y, width, height);
                    
                spriteBatch.Draw(Texture, destRect, sourceRect, Tint);
            }
        }
    }

    private void DrawCentered(SpriteBatch spriteBatch, Rectangle bounds)
    {
        int x = bounds.X + (bounds.Width - Texture.Width) / 2;
        int y = bounds.Y + (bounds.Height - Texture.Height) / 2;
            
        spriteBatch.Draw(Texture, new Vector2(x, y), Tint);
    }

    private void DrawFitted(SpriteBatch spriteBatch, Rectangle bounds, bool fitInside)
    {
        float scaleX = (float)bounds.Width / Texture.Width;
        float scaleY = (float)bounds.Height / Texture.Height;
        float scale = fitInside ? Math.Min(scaleX, scaleY) : Math.Max(scaleX, scaleY);
            
        int newWidth = (int)(Texture.Width * scale);
        int newHeight = (int)(Texture.Height * scale);
            
        int x = bounds.X + (bounds.Width - newWidth) / 2;
        int y = bounds.Y + (bounds.Height - newHeight) / 2;
            
        var destRect = new Rectangle(x, y, newWidth, newHeight);
        spriteBatch.Draw(Texture, destRect, Tint);
    }

    private void DrawNineSliced(SpriteBatch spriteBatch, Rectangle bounds)
    {
        // Asume que la textura tiene bordes de 1/3 para 9-slice
        // Implementación básica - se puede mejorar con márgenes configurables
        int sliceSize = Math.Min(Texture.Width, Texture.Height) / 3;
            
        // Esquinas (sin escalar)
        // Superior izquierda
        spriteBatch.Draw(Texture, 
            new Rectangle(bounds.X, bounds.Y, sliceSize, sliceSize),
            new Rectangle(0, 0, sliceSize, sliceSize), Tint);
            
        // Superior derecha
        spriteBatch.Draw(Texture, 
            new Rectangle(bounds.Right - sliceSize, bounds.Y, sliceSize, sliceSize),
            new Rectangle(Texture.Width - sliceSize, 0, sliceSize, sliceSize), Tint);
            
        // Inferior izquierda
        spriteBatch.Draw(Texture, 
            new Rectangle(bounds.X, bounds.Bottom - sliceSize, sliceSize, sliceSize),
            new Rectangle(0, Texture.Height - sliceSize, sliceSize, sliceSize), Tint);
            
        // Inferior derecha
        spriteBatch.Draw(Texture, 
            new Rectangle(bounds.Right - sliceSize, bounds.Bottom - sliceSize, sliceSize, sliceSize),
            new Rectangle(Texture.Width - sliceSize, Texture.Height - sliceSize, sliceSize, sliceSize), Tint);
            
        // Bordes (escalar en un eje)
        int centerWidth = bounds.Width - sliceSize * 2;
        int centerHeight = bounds.Height - sliceSize * 2;
            
        // Superior
        spriteBatch.Draw(Texture,
            new Rectangle(bounds.X + sliceSize, bounds.Y, centerWidth, sliceSize),
            new Rectangle(sliceSize, 0, Texture.Width - sliceSize * 2, sliceSize), Tint);
            
        // Inferior
        spriteBatch.Draw(Texture,
            new Rectangle(bounds.X + sliceSize, bounds.Bottom - sliceSize, centerWidth, sliceSize),
            new Rectangle(sliceSize, Texture.Height - sliceSize, Texture.Width - sliceSize * 2, sliceSize), Tint);
            
        // Izquierda
        spriteBatch.Draw(Texture,
            new Rectangle(bounds.X, bounds.Y + sliceSize, sliceSize, centerHeight),
            new Rectangle(0, sliceSize, sliceSize, Texture.Height - sliceSize * 2), Tint);
            
        // Derecha
        spriteBatch.Draw(Texture,
            new Rectangle(bounds.Right - sliceSize, bounds.Y + sliceSize, sliceSize, centerHeight),
            new Rectangle(Texture.Width - sliceSize, sliceSize, sliceSize, Texture.Height - sliceSize * 2), Tint);
            
        // Centro (escalar en ambos ejes)
        spriteBatch.Draw(Texture,
            new Rectangle(bounds.X + sliceSize, bounds.Y + sliceSize, centerWidth, centerHeight),
            new Rectangle(sliceSize, sliceSize, Texture.Width - sliceSize * 2, Texture.Height - sliceSize * 2), Tint);
    }
}

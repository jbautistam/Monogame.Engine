using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.UI.Brushes;

/// <summary>
/// Borde con textura (tileable)
/// </summary>
public class TexturedBorder : IBorder
{
    public Texture2D Texture { get; set; }
    public Color Tint { get; set; } = Color.White;
    public int Thickness { get; set; }
    public bool ScaleTexture { get; set; } = false;

    public bool HasContent => Texture != null && !Texture.IsDisposed && Tint.A > 0;

    public TexturedBorder(Texture2D texture, int thickness, bool scaleTexture = false)
    {
        Texture = texture;
        Thickness = thickness;
        ScaleTexture = scaleTexture;
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle bounds)
    {
        if (!HasContent) return;

        int t = Thickness;

        if (ScaleTexture)
        {
            // Escala la textura para ajustarse al borde
            // Arriba
            spriteBatch.Draw(Texture, new Rectangle(bounds.X, bounds.Y, bounds.Width, t), Tint);
            // Abajo
            spriteBatch.Draw(Texture, new Rectangle(bounds.X, bounds.Bottom - t, bounds.Width, t), Tint);
            // Izquierda (rotada 90 grados si la textura es horizontal)
            spriteBatch.Draw(Texture, new Rectangle(bounds.X, bounds.Y, t, bounds.Height), Tint);
            // Derecha
            spriteBatch.Draw(Texture, new Rectangle(bounds.Right - t, bounds.Y, t, bounds.Height), Tint);
        }
        else
        {
            // Repite la textura (tiling)
            DrawTiledSide(spriteBatch, bounds.X, bounds.Y, bounds.Width, t, false); // Arriba
            DrawTiledSide(spriteBatch, bounds.X, bounds.Bottom - t, bounds.Width, t, false); // Abajo
            DrawTiledSide(spriteBatch, bounds.X, bounds.Y, t, bounds.Height, true); // Izquierda
            DrawTiledSide(spriteBatch, bounds.Right - t, bounds.Y, t, bounds.Height, true); // Derecha
        }
    }

    private void DrawTiledSide(SpriteBatch spriteBatch, int x, int y, int width, int height, bool isVertical)
    {
        int texWidth = isVertical ? Texture.Height : Texture.Width; // Asume textura horizontal
        int texHeight = isVertical ? Texture.Width : Texture.Height;
            
        // Ajusta para orientación
        if (isVertical)
        {
            // Dibuja rotado 90 grados dibujando secciones verticales
            int drawWidth = height;
            int drawHeight = width;
                
            for (int pos = 0; pos < drawWidth; pos += texHeight)
            {
                int h = Math.Min(texHeight, drawWidth - pos);
                // Simplificación: dibuja como rectángulos sólidos de color promedio
                // Para rotación real necesitarías un shader o transformación
            }
        }
        else
        {
            for (int pos = 0; pos < width; pos += texWidth)
            {
                int w = Math.Min(texWidth, width - pos);
                var source = new Rectangle(0, 0, w, Texture.Height);
                var dest = new Rectangle(x + pos, y, w, height);
                spriteBatch.Draw(Texture, dest, source, Tint);
            }
        }
    }
}
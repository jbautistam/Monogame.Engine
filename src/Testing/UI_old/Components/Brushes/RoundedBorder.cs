using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.UI.Brushes;

/// <summary>
/// Borde con esquinas redondeadas
/// </summary>
public class RoundedBorder : IBorder
{
    public Color Color { get; set; }
    public int Thickness { get; set; } = 1;
    public float CornerRadius { get; set; } = 8f;

    public bool HasContent => Color.A > 0 && Thickness > 0 && CornerRadius > 0;

    public RoundedBorder(Color color, int thickness = 1, float cornerRadius = 8f)
    {
        Color = color;
        Thickness = thickness;
        CornerRadius = cornerRadius;
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle bounds)
    {
        if (!HasContent) return;

        // Simplificación: dibuja rectángulo con esquinas "recortadas" usando líneas
        // Para un dibujo perfecto de esquinas redondeadas se necesitaría geometría o shader
            
        var pixel = TextureHelper.GetPixelTexture(spriteBatch.GraphicsDevice);
        int t = Thickness;
        int r = (int)Math.Min(CornerRadius, Math.Min(bounds.Width, bounds.Height) / 2f);

        // Lados rectos (dejando espacio para esquinas)
        spriteBatch.Draw(pixel, new Rectangle(bounds.X + r, bounds.Y, bounds.Width - r * 2, t), Color);
        spriteBatch.Draw(pixel, new Rectangle(bounds.X + r, bounds.Bottom - t, bounds.Width - r * 2, t), Color);
        spriteBatch.Draw(pixel, new Rectangle(bounds.X, bounds.Y + r, t, bounds.Height - r * 2), Color);
        spriteBatch.Draw(pixel, new Rectangle(bounds.Right - t, bounds.Y + r, t, bounds.Height - r * 2), Color);

        // Esquinas (aproximación con pequeños rectángulos)
        DrawCorner(spriteBatch, pixel, bounds.X + r, bounds.Y + r, -1, -1, r, t);
        DrawCorner(spriteBatch, pixel, bounds.Right - r, bounds.Y + r, 1, -1, r, t);
        DrawCorner(spriteBatch, pixel, bounds.X + r, bounds.Bottom - r, -1, 1, r, t);
        DrawCorner(spriteBatch, pixel, bounds.Right - r, bounds.Bottom - r, 1, 1, r, t);
    }

    private void DrawCorner(SpriteBatch spriteBatch, Texture2D pixel, int cx, int cy, 
        int dirX, int dirY, int radius, int thickness)
    {
        // Aproximación simple: dibuja pequeños segmentos escalonados
        int steps = Math.Max(3, radius / 2);
            
        for (int i = 0; i < steps; i++)
        {
            float angle = MathHelper.PiOver2 * (i / (float)(steps - 1));
            int x = cx + (int)(dirX * (radius - Math.Sin(angle) * radius));
            int y = cy + (int)(dirY * (radius - Math.Cos(angle) * radius));
                
            spriteBatch.Draw(pixel, new Rectangle(x, y, thickness, thickness), Color);
        }
    }
}

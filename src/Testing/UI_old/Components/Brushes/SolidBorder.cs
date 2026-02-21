using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.UI.Brushes;

/// <summary>
/// Borde simple de línea continua con color uniforme
/// </summary>
public class SolidBorder : IBorder
{
    public Color Color { get; set; }
    public int Thickness { get; set; } = 1;

    public bool HasContent => Color.A > 0 && Thickness > 0;

    public SolidBorder(Color color, int thickness = 1)
    {
        Color = color;
        Thickness = thickness;
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle bounds)
    {
        if (!HasContent) return;

        var pixel = TextureHelper.GetPixelTexture(spriteBatch.GraphicsDevice);
        int t = Thickness;

        // Arriba
        spriteBatch.Draw(pixel, new Rectangle(bounds.X, bounds.Y, bounds.Width, t), Color);
        // Abajo
        spriteBatch.Draw(pixel, new Rectangle(bounds.X, bounds.Bottom - t, bounds.Width, t), Color);
        // Izquierda
        spriteBatch.Draw(pixel, new Rectangle(bounds.X, bounds.Y, t, bounds.Height), Color);
        // Derecha
        spriteBatch.Draw(pixel, new Rectangle(bounds.Right - t, bounds.Y, t, bounds.Height), Color);
    }
}

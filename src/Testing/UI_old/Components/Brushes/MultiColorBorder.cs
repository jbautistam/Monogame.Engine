using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.UI.Brushes;

/// <summary>
/// Borde con diferentes colores para cada lado
/// </summary>
public class MultiColorBorder : IBorder
{
    public Color TopColor { get; set; }
    public Color BottomColor { get; set; }
    public Color LeftColor { get; set; }
    public Color RightColor { get; set; }
    public int Thickness { get; set; } = 1;

    public bool HasContent => Thickness > 0 && (
        TopColor.A > 0 || BottomColor.A > 0 || 
        LeftColor.A > 0 || RightColor.A > 0);

    public MultiColorBorder(Color top, Color bottom, Color left, Color right, int thickness = 1)
    {
        TopColor = top;
        BottomColor = bottom;
        LeftColor = left;
        RightColor = right;
        Thickness = thickness;
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle bounds)
    {
        if (!HasContent) return;

        var pixel = TextureHelper.GetPixelTexture(spriteBatch.GraphicsDevice);
        int t = Thickness;

        if (TopColor.A > 0)
            spriteBatch.Draw(pixel, new Rectangle(bounds.X, bounds.Y, bounds.Width, t), TopColor);
        if (BottomColor.A > 0)
            spriteBatch.Draw(pixel, new Rectangle(bounds.X, bounds.Bottom - t, bounds.Width, t), BottomColor);
        if (LeftColor.A > 0)
            spriteBatch.Draw(pixel, new Rectangle(bounds.X, bounds.Y, t, bounds.Height), LeftColor);
        if (RightColor.A > 0)
            spriteBatch.Draw(pixel, new Rectangle(bounds.Right - t, bounds.Y, t, bounds.Height), RightColor);
    }
}


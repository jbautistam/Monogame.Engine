using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.UI.Brushes;

/// <summary>
/// Borde con sombra proyectada (simula profundidad)
/// </summary>
public class ShadowBorder : IBorder
{
    public Color ShadowColor { get; set; } = new Color(0, 0, 0, 128);
    public Vector2 Offset { get; set; } = new Vector2(4, 4);
    public int BlurRadius { get; set; } = 4;

    public int Thickness => (int)Math.Max(Math.Abs(Offset.X), Math.Abs(Offset.Y)) + BlurRadius;
    public bool HasContent => ShadowColor.A > 0 && (Offset != Vector2.Zero || BlurRadius > 0);

    public ShadowBorder(Color? shadowColor = null, Vector2? offset = null, int blurRadius = 4)
    {
        ShadowColor = shadowColor ?? new Color(0, 0, 0, 128);
        Offset = offset ?? new Vector2(4, 4);
        BlurRadius = blurRadius;
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle bounds)
    {
        if (!HasContent) return;

        // Simplificación: dibuja rectángulo sólido desplazado
        // Para blur real necesitarías un shader o render target
        var shadowBounds = new Rectangle(
            bounds.X + (int)Offset.X,
            bounds.Y + (int)Offset.Y,
            bounds.Width,
            bounds.Height
        );

        var pixel = TextureHelper.GetPixelTexture(spriteBatch.GraphicsDevice);
            
        // Dibuja sombra como un rectángulo semitransparente más grande
        if (BlurRadius > 0)
        {
            // Simula blur con capas concéntricas más transparentes
            for (int i = BlurRadius; i >= 0; i--)
            {
                var expanded = new Rectangle(
                    shadowBounds.X - i,
                    shadowBounds.Y - i,
                    shadowBounds.Width + i * 2,
                    shadowBounds.Height + i * 2
                );
                    
                float alpha = ShadowColor.A / 255f * (1f - (i / (float)(BlurRadius + 1)));
                var color = new Color(ShadowColor.R, ShadowColor.G, ShadowColor.B, (byte)(alpha * 255));
                    
                spriteBatch.Draw(pixel, expanded, color);
            }
        }
        else
        {
            spriteBatch.Draw(pixel, shadowBounds, ShadowColor);
        }
    }
}

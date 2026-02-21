using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.UI;

/// <summary>
/// Panel contenedor básico con fondo opcional
/// </summary>
public class Panel : UiContainerComponent
{
    public Color BackgroundColor { get; set; } = Color.Transparent;
    public Texture2D BackgroundTexture { get; set; }
    public Color BorderColor { get; set; } = Color.Transparent;
    public int BorderThickness { get; set; } = 1;
    public float CornerRadius { get; set; } = 0f; // Para rectángulos redondeados

    protected override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
    {
        var bounds = AbsoluteBounds;
            
        if (BackgroundColor != Color.Transparent || BackgroundTexture != null)
        {
            var rect = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
                
            if (BackgroundTexture != null)
            {
                spriteBatch.Draw(BackgroundTexture, rect, BackgroundColor);
            }
            else
            {
                // Dibuja rectángulo sólido usando una textura de 1x1 blanca
                var pixel = GetPixelTexture(spriteBatch.GraphicsDevice);
                spriteBatch.Draw(pixel, rect, BackgroundColor);
            }
        }

        if (BorderColor != Color.Transparent && BorderThickness > 0)
        {
            DrawBorder(spriteBatch, bounds, BorderColor, BorderThickness);
        }
    }

    private void DrawBorder(SpriteBatch spriteBatch, Rectangle bounds, Color color, int thickness)
    {
        var pixel = GetPixelTexture(spriteBatch.GraphicsDevice);
            
        // Arriba
        spriteBatch.Draw(pixel, new Rectangle(bounds.X, bounds.Y, bounds.Width, thickness), color);
        // Abajo
        spriteBatch.Draw(pixel, new Rectangle(bounds.X, bounds.Bottom - thickness, bounds.Width, thickness), color);
        // Izquierda
        spriteBatch.Draw(pixel, new Rectangle(bounds.X, bounds.Y, thickness, bounds.Height), color);
        // Derecha
        spriteBatch.Draw(pixel, new Rectangle(bounds.Right - thickness, bounds.Y, thickness, bounds.Height), color);
    }

    private static Texture2D _pixelTexture;
    private static Texture2D GetPixelTexture(GraphicsDevice device)
    {
        if (_pixelTexture == null || _pixelTexture.IsDisposed)
        {
            _pixelTexture = new Texture2D(device, 1, 1);
            _pixelTexture.SetData([ Color.White ]);
        }
        return _pixelTexture;
    }
}
